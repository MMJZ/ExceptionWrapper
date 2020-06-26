using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ExceptionWrapper
{
    public interface IAwaitable<out TResult>
    {
        public IAwaitResult<TResult> GetAwaiter();
    }

    public class AwaitResult<T> : IAwaitResult<T>
    {
        private readonly T _item;
        private readonly bool _isCompleted;

        public AwaitResult(T item, bool isCompleted)
        {
            _item = item;
            _isCompleted = isCompleted;
        }

        public void OnCompleted(Action continuation)
        {
            Console.WriteLine("OnCompleted");
            continuation();
        }

        public bool IsCompleted
        {
            get
            {
                Console.WriteLine($"figuring out if completed {_isCompleted} {_item}");
                return _isCompleted;
            }
        }

        public T GetResult() => _item;
    }

    public interface IAwaitResult<out T> : INotifyCompletion
    {
        public bool IsCompleted { get; }
        public T GetResult();
    }

    [AsyncMethodBuilder(typeof(TaskLikeMethodBuilder<>))]
    public class TaskLike<T> : IAwaitable<T>
    {
        private readonly T _item;
        private readonly bool _isCompleted;
        public TaskLike(T item, bool isCompleted)
        {
            _item = item;
            _isCompleted = isCompleted;
        }

        public IAwaitResult<T> GetAwaiter() => new AwaitResult<T>(_item, _isCompleted);
    }
}
