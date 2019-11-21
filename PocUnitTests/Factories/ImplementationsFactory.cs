
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
                HostName = "160.20.88.13",
                UserName = "vmrc",
                Password = "vmrc04",
                Port = 7001,
                TopicName = "PocRabbitMQParallelConsumer",
                SubscribeName = "PocRabbitMQParallelConsumer_01",
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
