using MessagingLib.Commons.Contracts;

namespace MessagingLib.Commons
{
    internal static class FactoryMessageControlRecovery
    {
        private static IConnectionManager GetConnectionInstance(string connectionString, string databaseName)
        {
            return new ConnectionManager(connectionString, databaseName);
        }

        internal static IMessageControlRepository GetMessageRepositoryInstance(string connectionString, string databaseName)
        {
            return new MessageControlRepository(GetConnectionInstance(connectionString, databaseName));
        }
    }
}