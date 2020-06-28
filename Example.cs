using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ExceptionWrapper
{
    public class Runner
    {
        public static async Task Main() => await new Runner().Run();

        private readonly Dictionary<string, int> _oddDictionary = new Dictionary<string, int>
        {
            ["first"] = 1,
            ["third"] = 3,
        };

        private readonly Dictionary<string, int> _evenDictionary = new Dictionary<string, int>
        {
            ["second"] = 2,
            ["fourth"] = 4,
        };

        private async Task Run()
        {
            (await Do())
                .Then(c => Console.Write($"Good Result!\n{c}"))
                .Catch(c => Console.Write($"Sad Result!\n{c.Print()}"));
        }

        private async Task<BaseResult<int>> Do()
        {
            // Tasks should also return BaseResults instead of throwing Exceptions
            var canShowNumber = await Task.Run(
                (Func<BaseResult<bool>>) (async () => true));

            // Handle async Tasks and async BaseResults in different async blocks
            return ((Func<BaseResult<int>>) (async () =>
            {
                var first = await First();
                var second = await Second(first);
                var number = await _oddDictionary.TryGetValue(second)
                    .TryRecover(_ => _evenDictionary.TryGetValue(second));
                return await canShowNumber ? number : 0;
            }))();
        }

        private static BaseResult<string> First()
        {
            // throw new OutOfMemoryException("Panicky exception");
            return new SuccessBaseResult<string>("sec");
        }

        private static async BaseResult<string> Second(string first)
        {
            // throw new DllNotFoundException("Other panicky exception");

            // Use async funcs to wrap up successful results automatically
            return first + "ond";
        }

        private static BaseResult<string> BadCall()
        {
            return new FailureBaseResult<string>(new BaseErrorMessage("V big panic"));
        }
    }
}
