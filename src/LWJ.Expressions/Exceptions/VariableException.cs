/**************************************************************
 *  Filename:    VariableException.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/
using System;

namespace LWJ.Expressions
{
    public class VariableException : ExpressionException
    {
        public VariableException(string message, string variableName)
            : this(message, variableName, null)
        {
        }

        public VariableException(string message, string variableName, Exception innerException)
            : base(message ?? Resource1.VarException, innerException)
        {
            this.VariableName = variableName;
        }

        public string VariableName { get; private set; }

        public override string Message
        {
            get
            {
                string message = base.Message;

                if (!string.IsNullOrEmpty(VariableName))
                    message += Environment.NewLine + Resource1.VariableName.FormatArgs(VariableName);

                return message;
            }
        }


    }
}
