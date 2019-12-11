using System;

namespace PocMessageria.Infrastructure.Messages
{
    /// <summary>
    /// INotification is a Interface for Notification type messages. This inherat IMessageBase propertys
    /// </summary>
    public interface INotification : IQueuedMessage
    {
         /// <summary>
        /// Id for Correlation Command that origin the action that create this event.
        /// This property is optional.
        /// </summary>
        Guid? CorrelationId { get; set; }
    }
}