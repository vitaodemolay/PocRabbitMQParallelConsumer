using System;
using MessageriaTests.TestInfra;
using MessageriaTests.TestInfra.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PocMessageria.BusImplementations.InMemory;
using PocMessageria.Infrastructure.Bus;

namespace MessageriaTests
{
    [TestClass]
    public class UnitTest1
    {
        private static IMessageRepository _messageRepository;
        private static IBus _inMemomoryBus;

        [ClassInitialize]
        public static void UnitTestInit(TestContext context)
        {
            _messageRepository = new MessageRepository();
            _inMemomoryBus = new InMemoryBus();
        }

        [ClassCleanup]
        public static void UnitTestCleanup()
        {
            _messageRepository = null;
            _inMemomoryBus = null;
        }


        [TestMethod]
        public void TestValidateSendCommand()
        {
            var handler = new TestCommandHandler(_messageRepository);

            var testMessage = new TestCommand(){
                HelloMessage = "Olá Mundo!",
                MessageId = Guid.NewGuid(),
                PublisherName = nameof(UnitTest1),
                Timestamp = DateTime.Now,
            };

            _inMemomoryBus.Receive(handler);

            _inMemomoryBus.Send(testMessage);


            Assert.AreEqual(testMessage, _messageRepository.GetMessageById(testMessage.MessageId));
        }

        
    }
}
