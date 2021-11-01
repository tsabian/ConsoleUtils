using System;

namespace Utilities.Term
{
    internal static class ProgramExtensions
    {
        internal static T GetArgValue<T>(this CommandArg current, string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentNullException(nameof(args), MessageConstants.ArgumentsFaultExceptionMessage);
            }

            var index = Array.IndexOf(args, current.Name);
            if (index == -1)
            {
                index = Array.IndexOf(args, current.ShortName);
            }
            if (index == -1)  return default;
            
            index++;

            try
            {
                return (T)Convert.ChangeType(args[index], typeof(T));
            }
            catch (IndexOutOfRangeException)
            {
                throw new CommandNullValueException(current);
            }
            catch (Exception)
            {
                return default;
            }
        }

        internal static ICommand CreateCommand(this Commands selectedCommand, string[] args)
        {
            ICommand command = selectedCommand switch
            {
                Commands.SendMail => new Programs.SendMail.SendMailCommand(args),
                Commands.CreateAccount => new Programs.CreateAccount.CreateAccountCommand(args),
                _ => null
            };
            return command;
        }
    }
}
