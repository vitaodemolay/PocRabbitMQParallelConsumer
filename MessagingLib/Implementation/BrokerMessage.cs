using RabbitMQ.Client;

namespace MessagingLib.Implementation
{
    internal class BrokerMessage
    {
        public string routingKey { get; set; }
        public byte[] body { get; set; }
        public IBasicProperties properties { get; set; }
    }
}
