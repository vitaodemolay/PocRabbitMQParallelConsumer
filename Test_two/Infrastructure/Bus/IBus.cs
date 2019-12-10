using PocMessageria.Infrastructure.Messages;

namespace PocMessageria.Infrastructure.Bus
{
    public interface IBus
    {
        void RegisterHandler<T>();

        void Send<T>(T command) where T : ICommand;

        void Publish<T>(T @event) where T : IEvent;

        void Notificate<T>(T notification) where T : INotification;

        void Subscribe<T>() where T : IEvent;

        void Unsubscribe<T>() where T : IEvent;

        void Receive<T>() where T: ICommand, INotification;
    }
}