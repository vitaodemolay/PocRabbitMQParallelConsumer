using MongoDB.Driver;
using System;

namespace MessagingLib.Commons.Contracts
{
    public interface IConnectionManager : IDisposable
    {
        IMongoDatabase GetDatabaseConnection { get; }
    }
}
