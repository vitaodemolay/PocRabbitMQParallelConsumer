using System;
using MongoDB.Bson.Serialization.Attributes;

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
        [BsonId()]
        Guid IdMessage { get; set; }

        
        string TimeStamping { get; set; }
    }
}
