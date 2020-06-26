using System;
using System.Runtime.CompilerServices;

namespace ExceptionWrapper
{
    public sealed class TaskLikeMethodBuilder<T>
    {
        private T _result;
        private bool _completed;

        public static TaskLikeMethodBuilder<T> Create()
        {
            Console.WriteLine("create");
            return new TaskLikeMethodBuilder<T>();
        }

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            Console.WriteLine("Start");
            stateMachine.MoveNext();
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            Console.WriteLine("SetStateMachine");
        }

        public void SetException(Exception exception)
        {
            Console.WriteLine("setException");
            Console.WriteLine(exception);
        }

        public void SetResult(T result)
        {
            Console.WriteLine("setResult");
            _result = result;
            _completed = true;
        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            Console.WriteLine("awaitOnCompleted");
            Console.WriteLine("running continuation");
            awaiter.OnCompleted(() => Console.WriteLine("hello"));
            Console.WriteLine("end continuation");
            _completed = false;
        }


        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            Console.WriteLine("awaitUnsafeOnCompleted");
            Console.WriteLine("running continuation");
            awaiter.OnCompleted(() => Console.WriteLine("hello"));
            Console.WriteLine("end continuation");
            _completed = false;
        }

        public TaskLike<T> Task
        {
            get
            {
                Console.WriteLine($"making new tasklike from {_result} {_completed}");
                return new TaskLike<T>(_result, _completed);
            }
        }
    }
}
