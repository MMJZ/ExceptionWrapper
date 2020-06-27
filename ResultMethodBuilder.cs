using System;
using System.Runtime.CompilerServices;

namespace ExceptionWrapper
{
    public sealed class ResultMethodBuilder<T>
    {
        private T _result = default!;
        private object _err = default!;
        private bool _hasResult;

        public static ResultMethodBuilder<T> Create() =>
            new ResultMethodBuilder<T>();

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine => stateMachine.MoveNext();

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }

        public void SetException(Exception exception) =>
            throw new Exception("PANIC", exception);

        public void SetResult(T result)
        {
            _result = result;
            _hasResult = true;
        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            if (awaiter is IAwaitResult<T, object> myAwaiter)
            {
                _err = myAwaiter.GetError();
            }
        }


        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            if (awaiter is IAwaitResult<T, object> myAwaiter)
            {
                _err = myAwaiter.GetError();
            }
        }

        // public SuccessBaseResult<T, TErr> Task => _hasResult
        //     ? new SuccessBaseResult<T, TErr>(_result)
        //     : (IBaseResult<T, TErr>) new FailureBaseResult<T, TErr>(_err);

        public SuccessBaseResult<T> Task => new SuccessBaseResult<T>(_result);
    }
}
