using System;

namespace LoxSharp.Exceptions
{
    public class RuntimeException : SystemException
    {
        public RuntimeException(Token token, string message) : base(message)
        {
            Token = token;
        }

        public Token Token { get; }
    }
}
