using System;
using CosumerSystemPoc.Commons.Contracts;

namespace CosumerSystemPoc.Domain
{
    public class MessageCommand : IRequest
    {
        public string SenderName { get; set; }
        public string BodyMessage { get; set; }
    }
}
