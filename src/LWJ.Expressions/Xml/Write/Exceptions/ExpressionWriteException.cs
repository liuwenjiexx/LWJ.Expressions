using System;

namespace LWJ.Expressions.Xml
{
    public class ExpressionWriteException : Exception
    {
        public ExpressionWriteException(string message, Expression expr)
            : this(message, expr, null)
        {
        }
        public ExpressionWriteException(string message, Expression expr, Exception innerException)
            : base(message ?? Resource1.Write_Exception, innerException)
        {
            this.Expression = expr;
            if (expr != null)
                this.ExpressionType = expr.ExpressionType;
            else
                this.ExpressionType = ExpressionType.None;
        }

        public ExpressionType ExpressionType { get; private set; }

        public Expression Expression { get; private set; }

        public override string Message
        {
            get
            {
                string message = base.Message;
                if (ExpressionType != ExpressionType.None)
                    message += Environment.NewLine + Resource1.Write_ExprType.FormatArgs(ExpressionType);
                return message;
            }
        }

    }
}
