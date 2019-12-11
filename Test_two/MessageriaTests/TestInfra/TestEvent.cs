using System;
using MessageriaTests.TestInfra.Contracts;
using PocMessageria.Infrastructure.Handler;
using PocMessageria.Infrastructure.Messages;

namespace MessageriaTests.TestInfra
{
    public class TestEvent : IEvent
    {
        public string PublisherName { get; set; }
        public Guid MessageId { get; set; }
        public DateTime Timestamp { get; set; }
        public string HelloMessage { get; set; }
        public Guid? CorrelationId { get; set; }
    }


    public class TestEventHandler : IHandler<TestEvent>
    {
        private readonly IMessageRepository _repository;

        public TestEventHandler(IMessageRepository repository)
        {
            _repository = repository;
        }

        public void Handle(TestEvent message)
        {
            if(message != null)
            {
                _repository.AddMessage(message);
            }
        }
    }
}
