using System;

namespace Utilities.Term
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Clear();

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
