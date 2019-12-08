using MessagingLib.Contracts;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static MessagingLib.Commons.StaticDefinitions;

namespace MessagingLib.Implementation
{
    public class Publisher : IPublisher
    {
        private readonly IConnection _connectionWithBroker;
        private readonly string _exchange;
        private readonly ISerializer _serializer;

        public Publisher(IBrokerConfiguration configurations, ISerializer serialize)
        {
            _exchange = configurations.TopicName;
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
        }


        public void Dispose()
        {
            if (_connectionWithBroker != null)
            {
                if (_connectionWithBroker.IsOpen) _connectionWithBroker.Close();

                _connectionWithBroker.Dispose();
            }
        }

        public Task SendAsync<T>(T message, DateTime? expireMessage = null)
        {
            var taskResult = new Task(() =>
            {
                using (var channel = _connectionWithBroker.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: _exchange, type: RabbitArgType);
                    var brokerMessage = BrokerMessageGenarate(message, channel.CreateBasicProperties(), expireMessage);
                    channel.BasicPublish(_exchange, brokerMessage.routingKey, brokerMessage.properties, brokerMessage.body);
                }
            });

            taskResult.Start();

            return taskResult;
        }

        private BrokerMessage BrokerMessageGenarate<T>(T message, IBasicProperties props, DateTime? expired = null)
        {
            var name = typeof(T).Name;
            string jsonMessage = _serializer.Serialize(message);

            var brokerMessage = new BrokerMessage
            {
                properties = props,
                routingKey = name,
                body = Encoding.UTF8.GetBytes(jsonMessage)
            };

            brokerMessage.properties.Type = name;
            brokerMessage.properties.MessageId = Guid.NewGuid().ToString();
            brokerMessage.properties.Headers = new Dictionary<string, object>();
            brokerMessage.properties.Headers.Add("Timestamp", DateTime.Now.ToString("O"));

            if (expired != null)
            {
                double expiration = (((DateTime)expired).ToUniversalTime().Subtract(DateTime.UtcNow)).TotalMilliseconds;
                brokerMessage.properties.Expiration = ((Int64)expiration).ToString("d");
            }

            return brokerMessage;
        }
    }
}
