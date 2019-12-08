using System;

namespace PocMessageria.Infrastructure.Messages
{
    /// <summary>
    /// IMessageBase is a aux interface for add default properts on Message types
    /// </summary>
    public interface IMessageBase
    {
        /// <summary>
        /// Unique Id for identity the message
        /// </summary>
        Guid MessageId { get; set; }

        /// <summary>
        /// Message creation timestamp
        /// </summary>
        DateTime Timestamp { get; set; }
    }
}
