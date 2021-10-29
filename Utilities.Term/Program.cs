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
            string commandArg = args[0];
            string commandParamArg = null;
            if (!Enum.TryParse(commandArg, out Commands command))
            {
                Console.WriteLine(MessageConstants.ArgumentsFaultExceptionMessage);
                return;
            }
            if (args.Length > 1)
            {
                commandParamArg = args[1];
            }
            var instance = command.CreateCommand(args);
            if (commandParamArg.Equals(CommandArgsConstants.HelpArg.Name) || commandParamArg.Equals(CommandArgsConstants.HelpArg.ShortName))
            {
                instance?.WriteHelp();
            }
            else
            {
                instance?.Execute();
                Console.WriteLine(MessageConstants.PressAnyKeyToContinueMessage);
            }
        }
    }
}
