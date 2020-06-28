using System;
using System.Net.Mail;

namespace ExceptionWrapper
{
    public static class Runner
    {
        public static void Main()
        {
            Do()
                .Then(c => Console.Write($"Good Result!\n{c}"))
                .Catch(c => Console.Write($"Oh no boys!\n{c.Print()}"));
        }

        private static async BaseResult<string> Do()
        {
            throw new SmtpException("I cannot possibly understand how to recover from this");
        }
    }
}
