using System.Runtime.ExceptionServices;

namespace ExceptionWrapper
{
    public abstract class BaseError
    {
        public BaseError? UnderlyingError { get; set; }
        public abstract string Print();
    }

    public class BaseException : BaseError
    {
        public ExceptionDispatchInfo Exception { get; }
        public BaseException(ExceptionDispatchInfo exception) => Exception = exception;
        public override string Print() => Exception.SourceException.ToString();
    }

    public class BaseErrorMessage : BaseError
    {
        public string Message { get; }
        public BaseErrorMessage(string message) => Message = message;
        public override string Print() => Message;
    }
}
