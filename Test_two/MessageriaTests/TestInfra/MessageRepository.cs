using System.Linq;
using System;
using System.Collections.Generic;
using MessageriaTests.TestInfra.Contracts;
using PocMessageria.Infrastructure.Messages;

namespace MessageriaTests.TestInfra
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IList<IMessageBase> _database;

        public MessageRepository(){
            _database = new List<IMessageBase>();
        }

        public void AddMessage(IMessageBase message)
        {
            if(_database.Any(f => f.MessageId == message.MessageId)){
                _database.Remove(_database.First(f => f.MessageId == message.MessageId));
            }

            _database.Add(message);
        }

        public IEnumerable<IMessageBase> GetAllMessageByType<T>()
        {
            return _database.Where(f => f.GetType().Equals(typeof(T)));
        }

        public IMessageBase GetMessageById(Guid messageId)
        {
            return _database.FirstOrDefault(f => f.MessageId == messageId);
        }
    }
}
