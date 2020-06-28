using System;
using System.Runtime.CompilerServices;

namespace ExceptionWrapper
{
    public interface IBaseResult<out T, out TErr>
    {
        IBaseResult<TU, TNErr> Bind<TU, TNErr>(Func<T, IBaseResult<TU, TNErr>> func,
            Func<TErr, IBaseResult<TU, TNErr>> errFunc);

        IBaseResult<T, TErr> Then(Action<T> action);
        IBaseResult<T, TErr> Catch(Action<TErr> action);

        IAwaitResult<T, TErr> GetAwaiter();
    }

    [AsyncMethodBuilder(typeof(ResultMethodBuilder<>))]
    public abstract class BaseResult<T> : IBaseResult<T, BaseError>
    {
        public abstract IBaseResult<TU, TNErr> Bind<TU, TNErr>(Func<T, IBaseResult<TU, TNErr>> func,
            Func<BaseError, IBaseResult<TU, TNErr>> errFunc);

        public abstract IBaseResult<T, BaseError> Then(Action<T> action);
        public abstract IBaseResult<T, BaseError> Catch(Action<BaseError> action);
        public abstract IAwaitResult<T, BaseError> GetAwaiter();
    }

    public class SuccessBaseResult<T> : BaseResult<T>
    {
        private readonly T _item;
        public SuccessBaseResult(T item) => _item = item;

        public override IBaseResult<TU, TNErr> Bind<TU, TNErr>(Func<T, IBaseResult<TU, TNErr>> func,
            Func<BaseError, IBaseResult<TU, TNErr>> errFunc) => func(_item);

        public override IBaseResult<T, BaseError> Then(Action<T> action)
        {
            action(_item);
            return this;
        }

        public override IBaseResult<T, BaseError> Catch(Action<BaseError> action) => this;
        public override IAwaitResult<T, BaseError> GetAwaiter() => new SuccessAwaitResult<T, BaseError>(_item);
    }

    public class FailureBaseResult<T> : BaseResult<T>
    {
        private readonly BaseError _err;
        public FailureBaseResult(BaseError err) => _err = err;

        public override IBaseResult<TU, TNErr> Bind<TU, TNErr>(Func<T, IBaseResult<TU, TNErr>> func,
            Func<BaseError, IBaseResult<TU, TNErr>> errFunc) => errFunc(_err);

        public override IBaseResult<T, BaseError> Then(Action<T> action) => this;

        public override IBaseResult<T, BaseError> Catch(Action<BaseError> action)
        {
            action(_err);
            return this;
        }

        public override IAwaitResult<T, BaseError> GetAwaiter() => new FailureAwaitResult<T, BaseError>(_err);
    }
}
