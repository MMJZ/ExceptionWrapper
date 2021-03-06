using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExceptionWrapper
{
    public static class F
    {
        public static BaseResult<TValue> TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key) where TKey : notnull => dictionary.TryGetValue(key, out var value)
            ? new SuccessBaseResult<TValue>(value)
            : (BaseResult<TValue>) new FailureBaseResult<TValue>(
                new BaseErrorMessage($"Key {key} not found in dictionary {nameof(dictionary)}"));

        public static IBaseResult<T, BaseError> TryRecover<T>(this BaseResult<T> result,
            Func<BaseError, BaseResult<T>> func) where T : notnull =>
            result.Bind(r => new SuccessBaseResult<T>(r), func);

        public static IBaseResult<T, BaseError> WithError<T>(this BaseResult<T> result,
            Func<BaseError, BaseResult<T>> func) where T : notnull => result.TryRecover(func);

        public static IBaseResult<T, BaseError> WithError<T>(this BaseResult<T> result, BaseError baseError)
            where T : notnull =>
            result.TryRecover(_ => new FailureBaseResult<T>(baseError));

        public static IBaseResult<BaseError> WithError(this BaseResult result, BaseError baseError) =>
            result.Bind(() => new SuccessBaseResult(), err => new FailureBaseResult(baseError));

        public static IBaseResult<T, BaseError> Bind<T>(this BaseResult<T> result,
            Func<T, BaseResult<T>> func) where T : notnull =>
            result.Bind(func, r => new FailureBaseResult<T>(r));


        public static BaseResult Require(this BaseResult result, bool condition) => condition
            ? new SuccessBaseResult()
            : (BaseResult) new FailureBaseResult(new BaseErrorMessage("Condition failed"));
    }
}
