using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ExceptionWrapper
{
    public static class Runner
    {
        private static Dictionary<string, int> _dictionary = new Dictionary<string, int>
        {
            ["first"] = 1,
            ["third"] = 3,
        };

        public static void Main()
        {
            var x = Do();
            if (x == null)
            {
                Console.WriteLine("x null");
            }
            x.Then(c => Console.Write($"Good Result!\n{c}"))
                .Catch(c =>
                {
                    if (c == null)
                    {
                        Console.WriteLine("c null");
                    }
                    Console.Write($"Oh no boys!\n{c?.Print()}");
                });
        }

        private static async BaseResult<int> Do()
        {
            var first = await First();
            var second = await Second(first);
            var number = await _dictionary.TryGetValue(second);


            return number;
        }

        private static async BaseResult<string> First()
        {
            Console.WriteLine("first");
            return "firs";
        }

        private static async BaseResult<string> Second(string first)
        {
            Console.WriteLine("second");
            throw new PathTooLongException("what am I even meant to do with this except crash");
            // return first + "st";
        }
    }
}
