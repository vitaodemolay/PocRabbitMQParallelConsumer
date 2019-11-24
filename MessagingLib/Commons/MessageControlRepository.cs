using MessagingLib.Commons.Contracts;
using MessagingLib.Domain;
using MongoDB.Driver;
using System;

namespace MessagingLib.Commons
{
    public class MessageControlRepository : IMessageControlRepository
    {

        private const string collectionName = "Messages";
        private readonly IConnectionManager _connectionManager;

        private IMongoCollection<Wrapper> _dbSet
        {
            get
            {
                var dbMongo = _connectionManager.GetDatabaseConnection;
                return dbMongo.GetCollection<Wrapper>(collectionName);
            }
        }
        

        public MessageControlRepository(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public void Dispose()
        {
            _connectionManager.Dispose();
        }

        public Wrapper GetWrapper(Guid messageId, string timestamp)
        {
            var wrappers = _dbSet.Find(f => f.IdMessage == messageId && f.TimeStamping == timestamp);
            return wrappers.FirstOrDefault();
        }

        public void SetWrapper(Wrapper wrapper)
        {
            _dbSet.InsertOne(wrapper);
        }
    }
}
