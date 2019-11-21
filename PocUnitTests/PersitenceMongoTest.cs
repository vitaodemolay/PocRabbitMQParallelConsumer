using MessagingLib.Commons;
using MessagingLib.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PocUnitTests.Factories;
using PocUnitTests.Implementations.Contracts;


namespace PocUnitTests
{
    [TestClass]
    public class PersitenceMongoTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var configs = ImplementationsFactory.GetBrokerConfigurationInstance();
            var connection = new ConnectionManager(configs.ConnectionString, configs.DabaseName);
            var messageRepository = new MessageControlRepository(connection);

            IRequest messageTest = new TestRequest(1, "José Anibal");

            messageRepository.SetMessage(messageTest).Wait();

        }
    }
}
