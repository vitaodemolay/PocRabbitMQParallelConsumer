using System;
using System.Collections.Generic;
using System.Linq;
using PocMessageria.Infrastructure.Bus;
using PocMessageria.Infrastructure.Handler;
using PocMessageria.Infrastructure.IoC;
using PocMessageria.Infrastructure.Messages;

namespace PocMessageria.BusImplementations.InMemory
{
    public class InMemoryBus : IBus
    {
        private readonly IList<Type> handlers = new List<Type>();
        private readonly IDependencyResolver dependencyResolver;

        public InMemoryBus(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
        }

        public void RegisterHandler<T>()
        {
            this.handlers.Add(typeof(T));
        }

        public void Send<T>(T command) where T : ICommand
        {
            Invoke<T>(command);
        }

        public void Publish<T>(T @event) where T : IEvent
        {
            Invoke<T>(@event);
        }

        public void Notificate<T>(T notification) where T : INotification
        {
            Invoke<T>(notification);
        }

        private void Invoke<T>(IMessageBase message) where T : IMessageBase
        {
            var handlerType = typeof(IHandler<>).MakeGenericType(typeof(T));

            foreach (var handler in handlers.Where(h => handlerType.IsAssignableFrom(h)))
                ((IHandler<T>)this.dependencyResolver.Get(handler)).Handle((T)message);
        }
    }
}
