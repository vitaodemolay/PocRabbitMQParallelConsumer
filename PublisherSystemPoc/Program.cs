using System;
using PublisherSystemPoc.Commons;
using PublisherSystemPoc.Domain;

namespace PublisherSystemPoc
{
    class Program
    {
        static void Main(string[] args)
        {
            var publisher = ImplementationsFactory.GetPublisherInstance();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Sistema de Envio de Mensagens");
                Console.WriteLine("=============================");

                Console.WriteLine("");
                Console.Write("Informe o Nome de quem envia a Mensagem: ");
                var _nome = Console.ReadLine();
                Console.Write("Quantas Mensagens Enviar: ");
                int _qtd = int.Parse(Console.ReadLine());

                Console.WriteLine("Enviando ...");

                for (int i = 1; i <= _qtd; i++)
                {
                    publisher.SendAsync(new MessageCommand
                    {
                        SenderName = _nome,
                        BodyMessage = $"Esta Mensagem é a numero {i}",
                    }).Wait();
                }

                Console.WriteLine($"Concluido. Foram enviadas {_qtd} mensagens");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.Write("Deseja executar mais uma vez?(S/N)-> ");
                var response = Console.ReadLine();

                if(response.ToUpper() != "S") break;
            }
        }
    }
}
