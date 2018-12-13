using System;

namespace LWJ.Expressions
{
    public class FunctionException : ExpressionException
    {
        public FunctionException(string message, string funcName)
            : this(message, funcName, null)
        {
        }

        public FunctionException(string message, string funcName, Exception innerException)
            : base(message ?? Resource1.FuncException, innerException)
        {
            this.FunctionName = funcName;
        }

        public string FunctionName { get; private set; }

        public override string Message
        {
            get
            {
                string message = base.Message;

                if (!string.IsNullOrEmpty(FunctionName))
                    message += Environment.NewLine + Resource1.FuncName.FormatArgs(FunctionName);

                return message;
            }
        }
    }
}
