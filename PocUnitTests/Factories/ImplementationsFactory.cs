
using MessagingLib.Contracts;
using MessagingLib.Implementation;
using PocUnitTests.Implementations;
using PocUnitTests.Implementations.Contracts;

namespace PocUnitTests.Factories
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

        public static IConsumer<IRequest, INotification> GetConsumerInstance()
        {
            return new Consumer<IRequest, INotification>(typeof(TestRequest), GetBrokerConfigurationInstance(), GetSerializerInstance());
        }
    }
}
