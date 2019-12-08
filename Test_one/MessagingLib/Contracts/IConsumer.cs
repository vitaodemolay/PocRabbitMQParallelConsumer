using System;

namespace MessagingLib.Contracts
{
    public interface IConsumer<IRequest, INotification> 
    {
        void OnMessage(Action<object> callback);
    }
}
