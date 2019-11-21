using MessagingLib.Contracts;
using System;
using System.Threading.Tasks;

namespace MessagingLib.Commons.Contracts
{
    public interface IMessageControlRepository : IDisposable
    {
        Task<IMessage> GetMessage(Guid messageId, string timestamp);
        Task SetMessage(IMessage message); 
    }
}
