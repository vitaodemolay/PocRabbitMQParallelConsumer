using System;

namespace MessagingLib.Contracts
{
    public interface IConsumer<IRequest, INotification> 
        where IRequest: Contracts.IRequest 
        where INotification: Contracts.INotification 
    {
        void OnMessage(Action<object> callback);
    }
}
