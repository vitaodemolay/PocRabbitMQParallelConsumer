using System;
using System.Collections.Generic;
using PocMessageria.Infrastructure.Bus;
using PocMessageria.Infrastructure.Handler;
using PocMessageria.Infrastructure.Messages;

namespace PocMessageria.BusImplementations.InMemory
{
    public class InMemoryBus : IBus
    {
        private readonly IDictionary<Type, object> _handlers;

        public InMemoryBus()
        {
            _handlers = new Dictionary<Type, object>();
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
            object handler = null;

            if (_handlers.TryGetValue(typeof(T), out handler))
            {
                ((IHandler<T>)handler).Handle((T)message);
            }
        }

        private void AddHandler<T>(IHandler<T> handler) where T : IMessageBase
        {
            _handlers.Add(typeof(T), handler);
        }

        public void Subscribe<T>(IHandler<T> handler) where T : IEvent
        {
            AddHandler(handler);
        }

        public void Unsubscribe<T>(IHandler<T> handler) where T : IEvent
        {
            _handlers.Remove(typeof(T));
        }

        public void Receive<T>(IHandler<T> handler) where T : IQueuedMessage
        {
            AddHandler(handler);
        }
    }
}
