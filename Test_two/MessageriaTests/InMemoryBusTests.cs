using MessageriaTests.TestInfra;
using MessageriaTests.TestInfra.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PocMessageria.BusImplementations.InMemory;
using PocMessageria.Infrastructure.Bus;
using System;
using System.Linq;

namespace MessageriaTests
{
    [TestClass]
    public class InMemoryBusTests
    {
        private IMessageRepository _messageRepository;
        private IBus _inMemomoryBus;

        [TestInitialize]
        public void TestInit()
        {
            _messageRepository = new MessageRepository();
            _inMemomoryBus = new InMemoryBus();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _messageRepository = null;
            _inMemomoryBus = null;
        }


        [TestMethod]
        public void ShouldValidateSuccessfulSendCommand()
        {
            var handler = new TestCommandHandler(_messageRepository);

            var testMessage = new TestCommand(){
                HelloMessage = "Olá Mundo!",
                MessageId = Guid.NewGuid(),
                PublisherName = nameof(InMemoryBusTests),
                Timestamp = DateTime.Now,
            };

            _inMemomoryBus.Receive(handler);

            _inMemomoryBus.Send(testMessage);


            Assert.AreEqual(testMessage, _messageRepository.GetMessageById(testMessage.MessageId));
        }

        [TestMethod]
        public void ShouldSendCommandButNotReceiveMessage()
        {
            var testMessage = new TestCommand(){
                HelloMessage = "Olá Mundo!",
                MessageId = Guid.NewGuid(),
                PublisherName = nameof(InMemoryBusTests),
                Timestamp = DateTime.Now,
            };

            _inMemomoryBus.Send(testMessage);


            Assert.IsNull(_messageRepository.GetMessageById(testMessage.MessageId));
        }

        
        [TestMethod]
        public void ShouldValidateSuccessfulPublishEvent()
        {
            var handler = new TestEventHandler(_messageRepository);

            var testMessage = new TestEvent(){
                HelloMessage = "Olá Mundo!",
                MessageId = Guid.NewGuid(),
                PublisherName = nameof(InMemoryBusTests),
                Timestamp = DateTime.Now,
            };

            _inMemomoryBus.Subscribe(handler);

            _inMemomoryBus.Publish(testMessage);

            _inMemomoryBus.Unsubscribe(handler);

            Assert.AreEqual(testMessage, _messageRepository.GetMessageById(testMessage.MessageId));
        }


        [TestMethod]
        public void ShouldPublishEventButNotReceiveMessage()
        {
            var handler = new TestEventHandler(_messageRepository);

            var testMessage = new TestEvent(){
                HelloMessage = "Olá Mundo!",
                MessageId = Guid.NewGuid(),
                PublisherName = nameof(InMemoryBusTests),
                Timestamp = DateTime.Now,
            };

            _inMemomoryBus.Subscribe(handler);
            _inMemomoryBus.Unsubscribe(handler);

            _inMemomoryBus.Publish(testMessage);

            Assert.IsNull(_messageRepository.GetMessageById(testMessage.MessageId));
        }


        [TestMethod]
        public void ShouldSendCommandAndReceiveASuccessNotification()
        {
            var commandHandler = new TestPublishNotificationCommandHandler(_inMemomoryBus);
            var notificationHandler = new TestNotificationHandler(_messageRepository);

            var testMessage = new TestPublishNotificationCommand()
            {
                ResultFail = false,
                MessageId = Guid.NewGuid(),
                PublisherName = nameof(InMemoryBusTests),
                Timestamp = DateTime.Now,
            };

            _inMemomoryBus.Receive(commandHandler);
            _inMemomoryBus.Receive<TestNotificationSuccess>(notificationHandler);
            _inMemomoryBus.Receive<TestNotificationFail>(notificationHandler);

            _inMemomoryBus.Send(testMessage);

            var _notifications = _messageRepository.GetAllMessageByType<TestNotificationSuccess>().ToList();

            Assert.AreEqual(1, _notifications.Count);
            Assert.AreEqual(testMessage.MessageId, ((TestNotificationSuccess)_notifications.First()).CorrelationId);
        }


        [TestMethod]
        public void ShouldSendCommandAndReceiveAFailNotification()
        {
            var commandHandler = new TestPublishNotificationCommandHandler(_inMemomoryBus);
            var notificationHandler = new TestNotificationHandler(_messageRepository);

            var testMessage = new TestPublishNotificationCommand()
            {
                ResultFail = true,
                MessageId = Guid.NewGuid(),
                PublisherName = nameof(InMemoryBusTests),
                Timestamp = DateTime.Now,
            };

            _inMemomoryBus.Receive(commandHandler);
            _inMemomoryBus.Receive<TestNotificationSuccess>(notificationHandler);
            _inMemomoryBus.Receive<TestNotificationFail>(notificationHandler);

            _inMemomoryBus.Send(testMessage);

            var _notifications = _messageRepository.GetAllMessageByType<TestNotificationFail>().ToList();

            Assert.AreEqual(1, _notifications.Count);
            Assert.AreEqual(testMessage.MessageId, ((TestNotificationFail)_notifications.First()).CorrelationId);
        }


        [TestMethod]
        public void ShouldSendCommandAndDoNotReceiveNotification()
        {
            var commandHandler = new TestPublishNotificationCommandHandler(_inMemomoryBus);
            var notificationHandler = new TestNotificationHandler(_messageRepository);

            var testMessage = new TestPublishNotificationCommand()
            {
                ResultFail = true,
                MessageId = Guid.NewGuid(),
                PublisherName = nameof(InMemoryBusTests),
                Timestamp = DateTime.Now,
            };

            _inMemomoryBus.Receive(commandHandler);

            _inMemomoryBus.Send(testMessage);

            var _notifications = _messageRepository.GetAllMessageByType<TestNotificationFail>().ToList();

            Assert.AreEqual(0, _notifications.Count);


            _notifications = _messageRepository.GetAllMessageByType<TestNotificationSuccess>().ToList();

            Assert.AreEqual(0, _notifications.Count);
        }
    }
}
