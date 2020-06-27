using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExceptionWrapper
{
    public static class Runner
    {
        public static async Task Main()
        {
            Console.Write($"Result: {await await DoToo()}");
        }

        private static async BaseResult<string> Do(string z)
        {
            var x = await new SuccessBaseResult<string>("success");
            return x + z;
        }

        private static async Task<IBaseResult<string, string>> DoToo()
        {
            var z = await Task.Run(() =>
            {
                Console.WriteLine("sleeping");
                Thread.Sleep(1000);
                Console.WriteLine("slept");
                return "taskdrop";
            });
            return Do(z);
        }
    }
}
