using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace ExceptionWrapper
{
    public sealed class ResultMethodBuilder<T>
    {
        private T _result = default!;
        private BaseError _err = default!;
        private bool _hasResult;

        public static ResultMethodBuilder<T> Create() =>
            new ResultMethodBuilder<T>();

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine => stateMachine.MoveNext();

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }

        public void SetException(Exception exception) =>
            _err = new BaseException(ExceptionDispatchInfo.Capture(exception));

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
            if (awaiter is IAwaitResult<T, BaseError> myAwaiter)
            {
                _err = myAwaiter.GetError();
            }
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            if (awaiter is IAwaitResult<T, BaseError> myAwaiter)
            {
                _err = myAwaiter.GetError();
            }
        }

        public BaseResult<T> Task => _hasResult
            ? new SuccessBaseResult<T>(_result)
            : (BaseResult<T>) new FailureBaseResult<T>(_err);
    }
}
