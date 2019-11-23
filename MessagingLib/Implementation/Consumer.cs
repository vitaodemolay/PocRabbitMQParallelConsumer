using MessagingLib.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static MessagingLib.Implementation.StaticDefinitions;

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


        public Consumer(Type assemblyBase, IBrokerConfiguration configurations, ISerializer serialize)
        {
            _exchange = configurations.TopicName;
            _subscribeName = configurations.SubscribeName;
            _serializer = serialize;


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

            var queueName = channel.QueueDeclare(queue:_subscribeName, durable: true, exclusive: false, autoDelete: false).QueueName;

            foreach (var bindingKey in _routingKeys)
            {
                channel.QueueBind(queue: queueName,
                                  exchange: _exchange,
                                  routingKey: bindingKey);
            }

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var type = ea.BasicProperties.Type;
                var body = Encoding.UTF8.GetString(ea.Body);

                var message = Deserialize(body, type);
                callback.Invoke(message);
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

            _channels.Add(channel);
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
