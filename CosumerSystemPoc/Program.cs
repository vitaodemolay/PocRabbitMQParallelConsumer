using System;
using CosumerSystemPoc.Commons;
using CosumerSystemPoc.Domain;

namespace CosumerSystemPoc
{
    class Program
    {
        static void Main(string[] args)
        {
            var consumer = ImplementationsFactory.GetConsumerInstance();

            Console.WriteLine("Sistema de Recebimento de Mensagens");
            Console.WriteLine("===================================");

            consumer.OnMessage(ExecOnReceiveOneMessage);

            while(true){
                
            }
        }

        private static void ExecOnReceiveOneMessage(dynamic message)
        {

            MessageCommand _messageRequest = message;

            Console.WriteLine($"{DateTime.Now} - {_messageRequest.SenderName} Diz: {_messageRequest.BodyMessage}");
        }
    }
}
