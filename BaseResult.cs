using System;
using System.Runtime.CompilerServices;

namespace ExceptionWrapper
{
    [AsyncMethodBuilder(typeof(ResultMethodBuilder<>))]
    public interface IBaseResult<out T, out TErr>
    {
        IBaseResult<TU, TNErr> Bind<TU, TNErr>(Func<T, IBaseResult<TU, TNErr>> func,
            Func<TErr, IBaseResult<TU, TNErr>> errFunc);

        IAwaitResult<T, TErr> GetAwaiter();
    }

    [AsyncMethodBuilder(typeof(ResultMethodBuilder<>))]
    public abstract class BaseResult<T> : IBaseResult<T, string>
    {
        public abstract IBaseResult<TU, TNErr> Bind<TU, TNErr>(Func<T, IBaseResult<TU, TNErr>> func,
            Func<string, IBaseResult<TU, TNErr>> errFunc);
        public abstract IAwaitResult<T, string> GetAwaiter();
    }

    public class SuccessBaseResult<T> : IBaseResult<T, string>
    {
        private readonly T _item;
        public SuccessBaseResult(T item) => _item = item;

        public IBaseResult<TU, TNErr> Bind<TU, TNErr>(Func<T, IBaseResult<TU, TNErr>> func,
            Func<string, IBaseResult<TU, TNErr>> errFunc) => func(_item);

        public IAwaitResult<T, string> GetAwaiter() => new SuccessAwaitResult<T, string>(_item);
    }

    public class FailureBaseResult<T, TErr> : IBaseResult<T, TErr>
    {
        private readonly TErr _err;
        public FailureBaseResult(TErr err) => _err = err;

        public IBaseResult<TU, TNErr> Bind<TU, TNErr>(Func<T, IBaseResult<TU, TNErr>> func,
            Func<TErr, IBaseResult<TU, TNErr>> errFunc) => errFunc(_err);

        public IAwaitResult<T, TErr> GetAwaiter() => new FailureAwaitResult<T, TErr>(_err);
    }
}
