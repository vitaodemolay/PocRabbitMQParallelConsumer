using System;

namespace PocMessageria.Infrastructure.Messages
{
    /// <summary>
    /// IEvent is a Interface for Events type messages. This inherat IMessageBase propertys
    /// </summary>
    public interface IEvent : IMessageBase
    {
        /// <summary>
        /// Id for Correlation Command that origin the action that create this event.
        /// This property is optional.
        /// </summary>
        Guid? CorrelationId { get; set; }
    }
}
