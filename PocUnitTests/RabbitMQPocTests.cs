using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PocUnitTests.Factories;
using PocUnitTests.Implementations.Contracts;

namespace PocUnitTests
{
    [TestClass]
    public class UnitTest1
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

            var publisherTask = new Task(async () =>
            {
                for (int i = 0; i < Names.Length; i++)
                {
                    await publisher.SendAsync(new TestRequest(i, Names[i]));
                }
            });

            publisherTask.Wait();

            Assert.AreEqual(Names.Length, BufferReceiveMessages.Count);
        }
    }
}
