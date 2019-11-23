using MessagingLib.Commons.Contracts;
using MessagingLib.Domain;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

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

        public async Task<Wrapper> GetWrapper(Guid messageId, string timestamp)
        {
            var wrappers = await _dbSet.FindAsync(f => f.IdMessage == messageId && f.TimeStamping == timestamp);
            return wrappers.FirstOrDefault();
        }

        public async Task SetWrapper(Wrapper wrapper)
        {
            await _dbSet.InsertOneAsync(wrapper);
        }
    }
}
