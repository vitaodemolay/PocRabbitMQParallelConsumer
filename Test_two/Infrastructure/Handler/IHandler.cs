using PocMessageria.Infrastructure.Messages;

namespace PocMessageria.Infrastructure.Handler
{
    public interface IHandler<T> where T : IMessageBase
    {
        void Handle(T @message);
    }
}