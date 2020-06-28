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

        public static IBaseResult<T, BaseError> Bind<T>(this BaseResult<T> result,
            Func<T, BaseResult<T>> func) where T : notnull =>
            result.Bind(func, r => new FailureBaseResult<T>(r));
    }
}
