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

        private static async TaskLike<string> Do(string z)
        {
            var x = await new TaskLike<string>("drop", true);
            var y = await new TaskLike<string>("drop", true);
            return x + y + z;
        }

        private static async Task<TaskLike<string>> DoToo()
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
