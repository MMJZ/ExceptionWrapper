using System;
using System.ComponentModel;
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

        public void SetException(Exception exception)
        {
            Console.WriteLine("SetException");
            _err = new BaseException(ExceptionDispatchInfo.Capture(exception));
        }

        public void SetResult(T result)
        {
            Console.WriteLine("SetResult");
            _result = result;
            _hasResult = true;
        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            if (awaiter is IAwaitResultErrorHolder<BaseError> baseErrorHolder)
            {
                _err = baseErrorHolder.GetError();
                Console.WriteLine($"AwaitUnsafeOnCompleted myAwaiter {_err}");
            }
            else
            {
                throw new InvalidAsynchronousStateException(
                    "Unable to pull error information about error from underlying type - are you" +
                    " awaiting another Type besides BaseResult?");
            }
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            if (awaiter is IAwaitResultErrorHolder<BaseError> baseErrorHolder)
            {
                _err = baseErrorHolder.GetError();
                Console.WriteLine($"AwaitUnsafeOnCompleted myAwaiter {_err}");
            }
            else
            {
                throw new InvalidAsynchronousStateException(
                    "Unable to pull error information about error from underlying type - are you" +
                    " awaiting another Type besides BaseResult?");
            }
        }

        public BaseResult<T> Task
        {
            get
            {

                Console.WriteLine($"Task {_hasResult} {_result} {_err}");

                if (_hasResult)
                {
                    return new SuccessBaseResult<T>(_result);
                }

                return (BaseResult<T>) new FailureBaseResult<T>(_err);
            }
        }
    }
}
