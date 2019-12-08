using CosumerSystemPoc.Commons.Contracts;
using CosumerSystemPoc.Domain;
using MessagingLib.Contracts;
using MessagingLib.Implementation;

namespace CosumerSystemPoc.Commons
{
    internal static class ImplementationsFactory
    {
        public static IBrokerConfiguration GetBrokerConfigurationInstance()
        {
            return new BrokerConfiguration
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                Port = 5672,
                TopicName = "PocRabbitMQParallelConsumer",
                SubscribeName = "PocRabbitMQParallelConsumer",
                VirtualHost = "",
                ConnectionString = "mongodb://admin:password@localhost:27017/admin",
                
            };
        }

        public static ISerializer GetSerializerInstance()
        {
            return new Serializer();
        }

        public static IConsumer<IRequest, INotification> GetConsumerInstance()
        {
            return new Consumer<IRequest, INotification>(typeof(MessageCommand), GetBrokerConfigurationInstance(), GetSerializerInstance());
        }
    }
}
