using MessagingLib.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PocUnitTests.Factories;
using PocUnitTests.Implementations.Contracts;
using System.Collections.Generic;
using System.Threading;

namespace PocUnitTests
{
    [TestClass]
    public class RabbitMQPocTests
    {
        private IList<IRequest> BufferReceiveMessages;
        private string[] Names = { "Jose", "Pedro", "Maria", "Joaquim", "Izaque", "Zaqueu", "Rosario" };

        private void ReceiveMessageCallback(dynamic message)
        {
            TestRequest _messageRequest = message;

            if (BufferReceiveMessages == null)
            {
                BufferReceiveMessages = new List<IRequest>();
            }

            BufferReceiveMessages.Add(_messageRequest);
        }

        [TestMethod]
        public void TestMethod1()
        {
            var publisher = ImplementationsFactory.GetPublisherInstance();
            var consumer = ImplementationsFactory.GetConsumerInstance();

            consumer.OnMessage(ReceiveMessageCallback);

            for (int i = 0; i < Names.Length; i++)
            {
                publisher.SendAsync(new TestRequest(i, Names[i])).Wait();
            }

            while (true)
            {
                if (BufferReceiveMessages != null && BufferReceiveMessages.Count > 0) break;
            }

            Thread.Sleep(2000);

            Assert.AreEqual(Names.Length, BufferReceiveMessages.Count);
        }
    }
}
