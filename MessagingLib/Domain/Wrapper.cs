using System;
using MongoDB.Bson.Serialization.Attributes;

namespace MessagingLib.Domain
{
    public class Wrapper
    {
        [BsonId]
        public Guid IdMessage { get; set; }

        [BsonRequired]
        public string TimeStamping { get; set; }

        [BsonIgnoreIfNull]
        public string TypeMessage { get; set; }

        [BsonIgnoreIfNull]
        public string BodyMessage { get; set; }
    }
}