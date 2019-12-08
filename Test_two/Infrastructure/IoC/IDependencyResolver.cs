using System;

namespace PocMessageria.Infrastructure.IoC
{
    public interface IDependencyResolver
    {
        T Get<T>();

        object Get(Type type);
    }
}