using MessagingLib.Domain;
using System;
using System.Threading.Tasks;

namespace MessagingLib.Commons.Contracts
{
    public interface IMessageControlRepository : IDisposable
    {
        Task<Wrapper> GetWrapper(Guid messageId, string timestamp);
        Task SetWrapper(Wrapper wrapper); 
    }
}
