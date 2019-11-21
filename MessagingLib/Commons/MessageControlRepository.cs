using MessagingLib.Commons.Contracts;
using MessagingLib.Contracts;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace MessagingLib.Commons
{
    public class MessageControlRepository : IMessageControlRepository
    {

        private const string collectionName = "Messages";
        private readonly IConnectionManager _connectionManager;

        private IMongoCollection<IMessage> _dbSet
        {
            get
            {
                var dbMongo = _connectionManager.GetDatabaseConnection;
                return dbMongo.GetCollection<IMessage>(collectionName);
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

        public async Task<IMessage> GetMessage(Guid messageId, string timestamp)
        {
            var messages = await _dbSet.FindAsync(f => f.IdMessage == messageId && f.TimeStamping == timestamp);
            return messages.FirstOrDefault();
        }

        public async Task SetMessage(IMessage message)
        {
            await _dbSet.InsertOneAsync(message);
        }
    }
}
