using System;
using System.Runtime.CompilerServices;

namespace ExceptionWrapper
{
    public interface IAwaitResult<out T, out TErr> : ICriticalNotifyCompletion
    {
        bool IsCompleted { get; }
        T GetResult();
        TErr GetError();
    }

    public readonly struct SuccessAwaitResult<T, TErr> : IAwaitResult<T, TErr>
    {
        private readonly T _item;
        public SuccessAwaitResult(T item) => _item = item;
        public void OnCompleted(Action continuation) => continuation();
        public void UnsafeOnCompleted(Action continuation) => continuation();
        public bool IsCompleted => true;
        public T GetResult() => _item;
        public TErr GetError() => throw new InvalidOperationException();
    }

    public readonly struct FailureAwaitResult<T, TErr> : IAwaitResult<T, TErr>
    {
        private readonly TErr _err;
        public FailureAwaitResult(TErr err) => _err = err;

        public void OnCompleted(Action continuation)
        {
        }

        public void UnsafeOnCompleted(Action continuation)
        {
        }

        public bool IsCompleted => false;
        public T GetResult() => throw new InvalidOperationException();
        public TErr GetError() => _err;
    }
}
