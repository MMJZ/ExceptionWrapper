using System;

namespace ExceptionWrapper
{
    public class Ext
    {
        public IResult<TOut> Do<T, TU, TV, TOut>(IResult<T> rt, IResult<TU> rtu, IResult<TV> rtv,
            Func<T, TU, TV, IResult<TOut>> func) =>
            rt.Bind(t =>
                rtu.Bind(tu =>
                    rtv.Bind(tv =>
                        func(t, tu, tv))));
    }
}