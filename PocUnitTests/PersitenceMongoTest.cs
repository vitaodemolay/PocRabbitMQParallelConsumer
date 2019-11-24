using System;
using MessagingLib.Commons;
using MessagingLib.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PocUnitTests.Factories;


namespace PocUnitTests
{
    [TestClass]
    public class PersitenceMongoTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var configs = ImplementationsFactory.GetBrokerConfigurationInstance();
            var connection = new ConnectionManager(configs.ConnectionString, configs.SubscribeName);
            var messageRepository = new MessageControlRepository(connection);

            var wrapper = new Wrapper{
                IdMessage = Guid.NewGuid(),
                TimeStamping = DateTime.Now.ToString("O"),
            };

            messageRepository.SetWrapper(wrapper);

        }
    }
}
