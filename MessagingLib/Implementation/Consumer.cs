using MessagingLib.Commons;
using MessagingLib.Commons.Contracts;
using MessagingLib.Contracts;
using MessagingLib.Domain;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static MessagingLib.Commons.StaticDefinitions;

namespace MessagingLib.Implementation
{
    public class Consumer<IRequest, INotification> : IConsumer<IRequest, INotification>, IDisposable
    {

        private readonly IConnection _connectionWithBroker;
        private readonly IList<IModel> _channels;
        private readonly string _exchange;
        private readonly string _subscribeName;
        private readonly string[] _routingKeys;
        private readonly Dictionary<Type, string> _messageTypeAddresses = new Dictionary<Type, string>();
        private readonly ISerializer _serializer;
        private readonly IMessageControlRepository _messageRepository;

        public Consumer(Type assemblyBase, IBrokerConfiguration configurations, ISerializer serialize)
        {
            _exchange = configurations.TopicName;
            _subscribeName = configurations.SubscribeName;
            _serializer = serialize;
            _messageRepository = FactoryMessageControlRecovery.GetMessageRepositoryInstance(configurations.ConnectionString, configurations.DabaseName);

            var connectionFactory = new ConnectionFactory
            {
                HostName = configurations.HostName,
            };

            if (configurations.Port != null)
            {
                connectionFactory.Port = configurations.Port.Value;
            }

            if (!string.IsNullOrEmpty(configurations.UserName))
                connectionFactory.UserName = configurations.UserName;

            if (!string.IsNullOrEmpty(configurations.Password))
                connectionFactory.Password = configurations.Password;

            if (!string.IsNullOrEmpty(configurations.VirtualHost))
                connectionFactory.VirtualHost = configurations.VirtualHost;

            _connectionWithBroker = connectionFactory.CreateConnection();

            _channels = new List<IModel>();

            var assembly = assemblyBase.Assembly;

            foreach (var requestTypeAddress in assembly.GetTypes().Where(filterType => filterType.GetInterfaces().Contains(typeof(IRequest))))
            {
                _messageTypeAddresses.Add(requestTypeAddress, requestTypeAddress.Name);
            }

            foreach (var notificationTypeAddress in assembly.GetTypes().Where(filterType => filterType.GetInterfaces().Contains(typeof(INotification))))
            {
                _messageTypeAddresses.Add(notificationTypeAddress, notificationTypeAddress.Name);
            }

            _routingKeys = (from query in _messageTypeAddresses select query.Value).ToArray();
        }


        public void OnMessage(Action<object> callback)
        {
            var channel = _connectionWithBroker.CreateModel();
            channel.ExchangeDeclare(exchange: _exchange, type: RabbitArgType);

            var queueName = channel.QueueDeclare(queue: _subscribeName, durable: true, exclusive: false, autoDelete: false).QueueName;

            foreach (var bindingKey in _routingKeys)
            {
                channel.QueueBind(queue: queueName,
                                  exchange: _exchange,
                                  routingKey: bindingKey);
            }

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var persistResult = PersistUniqueWrapper(ea);
                callback.Invoke(persistResult.Message);
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

            _channels.Add(channel);
        }

        private PersistUniqueWrapperResult PersistUniqueWrapper(BasicDeliverEventArgs deliverEventArgs)
        {
            var result = new PersistUniqueWrapperResult
            {
                IsPersisted = false,
            };

            Guid idMessage = Guid.Empty;

            if(!Guid.TryParse(deliverEventArgs.BasicProperties.MessageId, out idMessage)){
                throw new Exception("Null reference Message ID");
            }

            var wrapper = new Wrapper
            {
                IdMessage = idMessage,
                TimeStamping = deliverEventArgs.BasicProperties.Headers.FirstOrDefault(f => f.Key == "Timestamp").Value.ToString(),
                TypeMessage = deliverEventArgs.BasicProperties.Type,
                BodyMessage = Encoding.UTF8.GetString(deliverEventArgs.Body)
            };
            try
            {
                _messageRepository.SetWrapper(wrapper);
                result.Message = Deserialize(wrapper.BodyMessage, wrapper.TypeMessage);
                result.IsPersisted = true;
            }
            catch (System.Exception)
            {
            
            }

            return result;
        }

        private object Deserialize(string messageBody, string type)
        {
            Type typeMessage = _messageTypeAddresses.Where(f => f.Value == type).SingleOrDefault().Key;
            var method = _serializer.GetType().GetMethod("Deserialize");
            var genericMethod = method.MakeGenericMethod(typeMessage);

            var messageResult = genericMethod.Invoke(_serializer, new object[] { messageBody });

            return messageResult;
        }

        public void Dispose()
        {
            if (_channels.Count > 0)
            {
                foreach (var channel in _channels)
                {
                    if (channel.IsOpen) channel.Close();

                    channel.Dispose();
                }
            }
            if (_connectionWithBroker != null)
            {
                if (_connectionWithBroker.IsOpen) _connectionWithBroker.Close();

                _connectionWithBroker.Dispose();
            }
        }
    }
}
