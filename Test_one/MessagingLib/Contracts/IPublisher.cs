using System;
using System.Threading.Tasks;

namespace MessagingLib.Contracts
{
    public interface IPublisher : IDisposable
    {
        Task SendAsync<T>(T message, DateTime? expireMessage = null);
    }
}
