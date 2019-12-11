using System;
using MessageriaTests.TestInfra.Contracts;
using PocMessageria.Infrastructure.Handler;
using PocMessageria.Infrastructure.Messages;

namespace MessageriaTests.TestInfra
{
    public class TestNotificationSuccess : INotification
    {
        public TestNotificationSuccess(ICommand command)
        {
            CorrelationId = command.MessageId;
            MessageId = Guid.NewGuid();
            Timestamp = DateTime.Now;
        }

        public Guid? CorrelationId { get; set; }
        public Guid MessageId { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class TestNotificationFail : INotification
    {
        public TestNotificationFail(ICommand command)
        {
            CorrelationId = command.MessageId;
            MessageId = Guid.NewGuid();
            Timestamp = DateTime.Now;
        }

        public Guid? CorrelationId { get; set; }
        public Guid MessageId { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class TestNotificationHandler :
        IHandler<TestNotificationSuccess>,
        IHandler<TestNotificationFail>
    {
        private readonly IMessageRepository _repository;

        public TestNotificationHandler(IMessageRepository repository)
        {
            _repository = repository;
        }

        public void Handle(TestNotificationSuccess message)
        {
            if (message != null)
            {
                _repository.AddMessage(message);
            }
        }

        public void Handle(TestNotificationFail message)
        {
            if (message != null)
            {
                _repository.AddMessage(message);
            }
        }
    }
}