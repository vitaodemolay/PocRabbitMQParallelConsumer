
namespace PocMessageria.Infrastructure.Messages
{
    /// <summary>
    /// ICommand is a Interface for Commands type messages. This inherat IMessageBase propertys
    /// </summary>
    public interface ICommand : IQueuedMessage
    {
        /// <summary>
        /// This property is the publisher system name that origin this command. 
        /// </summary>
        string PublisherName { get; set; }
    }
}
