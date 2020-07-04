using System;
using System.Collections.Generic;

namespace ExceptionWrapper
{
    public class Runner
    {
        public static void Main() => new Runner().Run();

        private readonly Dictionary<string, int> _oddDictionary = new Dictionary<string, int>
        {
            ["first"] = 1,
            ["third"] = 3
        };

        private readonly Dictionary<string, int> _evenDictionary = new Dictionary<string, int>
        {
            ["second"] = 2,
            ["fourth"] = 4
        };

        private void Run()
        {
            GetDictionaryLookupSum()
                .Then(c => Console.Write($"Good Result!\n{c}"))
                .Catch(c => Console.Write($"Sad Result!\n{c?.Print()}"));
        }

        private async BaseResult<int> GetDictionaryLookupSum()
        {
            var first = await First()
                .TryRecover(async err =>
                {
                    await Guard
                        .Require(err is BaseException ex
                                 && ex.Exception.SourceException is OutOfMemoryException)
                        .WithError(new BaseErrorMessage("Unable to recover if not OOM Exception"));
                    return "first";
                });

            var firstLookup = await _oddDictionary
                .TryGetValue(first)
                .WithError(new BaseErrorMessage("failed to do odd numbers dictionary"));

            await Guard
                .Require(firstLookup > 0)
                .WithError(new BaseErrorMessage("needed positive value from dictionary"));

            var second = await Second(first);
            var secondLookup = _evenDictionary
                .TryGetValue(second)
                // Bad idea to default value without logging when it happens...
                .Catch(_ => Console.WriteLine("Defaulting lookup on evenDictionary - key not found"))
                .UnwrapOrDefault(5);

            return firstLookup + secondLookup;
        }

        // If this function isn't async, the thrown exception will bubble past the Guard above...
        private static async BaseResult<string> First()
        {
           throw new OutOfMemoryException("Panicky exception");
           // throw new Exception("general exception");
           // return "first";
        }

        private static async BaseResult<string> Second(string first)
        {
            // Use async signatures to wrap up successful results automatically
            return first + "-bad-lookup-attempt";
            // return "second";
        }
    }
}
