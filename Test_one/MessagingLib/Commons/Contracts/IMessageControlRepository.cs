using MessagingLib.Domain;
using System;
using System.Threading.Tasks;

namespace MessagingLib.Commons.Contracts
{
    public interface IMessageControlRepository : IDisposable
    {
        Wrapper GetWrapper(Guid messageId, string timestamp);
        void SetWrapper(Wrapper wrapper); 
    }
}
