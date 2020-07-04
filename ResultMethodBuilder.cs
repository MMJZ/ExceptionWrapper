using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace ExceptionWrapper
{

    public sealed class ResultMethodBuilder
    {
        private BaseError _err = default!;
        private bool _hasResult;

        public static ResultMethodBuilder Create() =>
            new ResultMethodBuilder();

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine => stateMachine.MoveNext();

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }

        public void SetException(Exception exception) =>
            _err = new BaseException(ExceptionDispatchInfo.Capture(exception));

        public void SetResult()
        {
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
            }
            else
            {
                throw new InvalidAsynchronousStateException(
                    "Unable to pull error information about error from underlying type - are you" +
                    " awaiting another Type besides BaseResult?");
            }
        }

        public BaseResult Task =>
            _hasResult
                ? new SuccessBaseResult()
                : (BaseResult) new FailureBaseResult(_err);
    }

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
            if (awaiter is IAwaitResultErrorHolder<BaseError> baseErrorHolder)
            {
                _err = baseErrorHolder.GetError();
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
            }
            else
            {
                throw new InvalidAsynchronousStateException(
                    "Unable to pull error information about error from underlying type - are you" +
                    " awaiting another Type besides BaseResult?");
            }
        }

        public BaseResult<T> Task =>
            _hasResult
                ? new SuccessBaseResult<T>(_result)
                : (BaseResult<T>) new FailureBaseResult<T>(_err);
    }
}
