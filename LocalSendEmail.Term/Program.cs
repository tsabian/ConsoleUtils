using System;

namespace LocalSendEmail.Term
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(MessageConstants.ArgumentsFaultExceptionMessage);
                return;
            }

            SendMailCommand command = new(args);
            command.Execute();

            Console.WriteLine(MessageConstants.PressAnyKeyToContinueMessage);
        }
    }
}
