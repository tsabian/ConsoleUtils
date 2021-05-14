using System;

namespace LocalSendEmail.Term
{
    internal static class ProgramExtensions
    {

        internal static T GetArgValue<T>(this CommandArg current, string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentNullException();
            }

            var index = Array.IndexOf(args, current.Name);
            if (index == -1)
            {
                index = Array.IndexOf(args, current.ShortName);
            }

            if (index == -1)
            {
                return default;
            }

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

    }
}
