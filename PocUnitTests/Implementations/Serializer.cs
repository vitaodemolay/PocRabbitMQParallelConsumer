using Jil;
using MessagingLib.Contracts;

namespace PocUnitTests.Implementations
{
    internal class Serializer : ISerializer
    {
        public T Deserialize<T>(string json)
        {
            return JSON.Deserialize<T>(json);
        }

        public string Serialize<T>(T obj)
        {
            return JSON.Serialize(obj);
        }
    }
}
