using MessagingLib.Contracts;

namespace PocUnitTests.Implementations
{
    internal class BrokerConfiguration : IBrokerConfiguration
    {
        public string TopicName { get; set; }
        public string HostName { get; set; }
        public int? Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string SubscribeName { get; set; }
        public string ConnectionString { get; set; }
    }
}
