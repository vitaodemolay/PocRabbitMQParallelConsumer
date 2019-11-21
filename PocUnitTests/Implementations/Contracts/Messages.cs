using MessagingLib.Contracts;
using System;

namespace PocUnitTests.Implementations.Contracts
{
    public abstract class BaseMessage : IMessage
    {
        public Guid IdMessage { get; set; }
        public string TimeStamping { get; set; }

        public BaseMessage()
        {
            IdMessage = Guid.NewGuid();
            TimeStamping = DateTime.Now.ToString("O");
        }
    }

    public class TestRequest : BaseMessage, IRequest
    {
        public int Index { get; set; }
        public string Name { get; set; }

        public TestRequest()
            :base()
        { }


        public TestRequest(int index, string name)
         : base()
        {
            Index = index;
            Name = name;
        }
    }
}
