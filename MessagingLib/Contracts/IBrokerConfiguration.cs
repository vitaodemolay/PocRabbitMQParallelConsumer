namespace MessagingLib.Contracts
{
    public interface IBrokerConfiguration
    {
        string TopicName { get; set; }
        string HostName { get; set; }
        int? Port { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string VirtualHost { get; set; }
        string SubscribeName { get; set; }
        string ConnectionString { get; set; }
    }
}
