using MessagingLib.Contracts;
using MessagingLib.Implementation;

namespace PublisherSystemPoc.Commons
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
            };
        }

        public static ISerializer GetSerializerInstance()
        {
            return new Serializer();
        }

        public static IPublisher GetPublisherInstance()
        {
            return new Publisher(GetBrokerConfigurationInstance(), GetSerializerInstance());
        }
    }
}
