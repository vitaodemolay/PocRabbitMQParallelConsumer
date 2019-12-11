using PocMessageria.Infrastructure.Handler;
using PocMessageria.Infrastructure.Messages;

namespace PocMessageria.Infrastructure.Bus
{
    public interface IBus
    {
        void Send<T>(T command) where T : ICommand;

        void Publish<T>(T @event) where T : IEvent;

        void Notificate<T>(T notification) where T : INotification;

        void Subscribe<T>(IHandler<T> handler) where T : IEvent;

        void Unsubscribe<T>(IHandler<T> handler) where T : IEvent;

        void Receive<T>(IHandler<T> handler) where T: IQueuedMessage;

    }
}