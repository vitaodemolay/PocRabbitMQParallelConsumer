using System;
using System.Collections.Generic;
using PocMessageria.Infrastructure.Messages;

namespace MessageriaTests.TestInfra.Contracts
{
    public interface IMessageRepository
    {
        void AddMessage(IMessageBase message);
        IMessageBase GetMessageById(Guid messageId);
        IEnumerable<IMessageBase> GetAllMessageByType<T>();
    }
}
