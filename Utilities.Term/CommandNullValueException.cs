using System;
namespace Utilities.Term
{
    internal class CommandNullValueException : Exception
    {
        private readonly CommandArg _command;

        internal CommandNullValueException(CommandArg command) : 
            base($"Necessário informar o valor do commando {command.Name} ou {command.ShortName}")
        {
            this._command = command;
        }
    }
}
