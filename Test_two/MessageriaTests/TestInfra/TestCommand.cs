using System;
using MessageriaTests.TestInfra.Contracts;
using PocMessageria.Infrastructure.Handler;
using PocMessageria.Infrastructure.Messages;

namespace MessageriaTests.TestInfra
{
    public class TestCommand : ICommand
    {
        public string PublisherName { get; set; }
        public Guid MessageId { get; set; }
        public DateTime Timestamp { get; set; }
        public string HelloMessage { get; set; }
    }


    public class TestCommandHandler : IHandler<TestCommand>
    {
        private readonly IMessageRepository _repository;

        public TestCommandHandler(IMessageRepository repository)
        {
            _repository = repository;
        }

        public void Handle(TestCommand message)
        {
            if(message != null)
            {
                _repository.AddMessage(message);
            }
        }
    }
}
