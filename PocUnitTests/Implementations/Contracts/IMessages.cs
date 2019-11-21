using System;

namespace PocUnitTests.Implementations.Contracts
{
    public interface IRequest : IMessage
    {

    }

    public interface INotification : IMessage
    {

    }

    public interface IMessage
    {
        Guid IdMessage { get; set; }
        string TimeStamping { get; set; }
    }
}
