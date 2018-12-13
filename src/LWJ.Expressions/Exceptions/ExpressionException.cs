using System;

namespace LWJ.Expressions
{
    public class ExpressionException : Exception
    {
        public ExpressionException(string message)
            : this(message, null)
        {
        }
        public ExpressionException(string message, Exception innerException)
            : base(message ?? Resource1.ExprException, innerException)
        {
        }

    }
}
