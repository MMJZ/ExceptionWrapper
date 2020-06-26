using System;

namespace ExceptionWrapper
{
    public interface IResult<out T>
    {
        IResult<TU> Bind<TU>(Func<T, IResult<TU>> func);
    }
}