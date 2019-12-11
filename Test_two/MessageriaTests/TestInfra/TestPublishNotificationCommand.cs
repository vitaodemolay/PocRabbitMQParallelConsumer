using System;
using PocMessageria.Infrastructure.Bus;
using PocMessageria.Infrastructure.Handler;
using PocMessageria.Infrastructure.Messages;

namespace MessageriaTests.TestInfra
{
    public class TestPublishNotificationCommand : ICommand
    {
        public string PublisherName { get; set; }
        public Guid MessageId { get; set; }
        public DateTime Timestamp { get; set; }
        public bool ResultFail { get; set; }
    }


    public class TestPublishNotificationCommandHandler : IHandler<TestPublishNotificationCommand>
    {
        private readonly IBus _bus;

        public TestPublishNotificationCommandHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(TestPublishNotificationCommand message)
        {
            if (message != null)
            {
                try
                {
                    if (message.ResultFail) { throw new Exception(); }

                    _bus.Notificate(new TestNotificationSuccess(message));
                }
                catch (System.Exception)
                {
                   _bus.Notificate(new TestNotificationFail(message));
                }
            }
        }
    }
}
