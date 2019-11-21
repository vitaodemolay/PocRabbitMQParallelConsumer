using MessagingLib.Commons.Contracts;
using MessagingLib.Contracts;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MessagingLib.Commons
{
    public class ConnectionManager : IConnectionManager
    {
        private readonly string _connectionstring;
        private readonly string _databaseName;

        private MongoClient _client { get; set; }

        public ConnectionManager(string connectionstring, string databaseName)
        {
            _connectionstring = connectionstring;
            _databaseName = databaseName;
        }

        public IMongoDatabase GetDatabaseConnection => GetDatabaseConnectionImplementations();


        public void Dispose()
        {
            _client = null;
        }

        private IMongoDatabase GetDatabaseConnectionImplementations()
        {
            if(_client == null) CreateClient();

            return _client.GetDatabase(_databaseName);
        }

        private void CreateClient()
        {
            OnModelBuilder();
            _client = new MongoClient(_connectionstring);
        }


        private void OnModelBuilder()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(IMessage)))
                BsonClassMap.RegisterClassMap<IMessage>(model =>
                {
                    model.AutoMap();
                    model.SetIdMember(model.GetMemberMap(m => m.IdMessage));
                });



        }

    }
}
