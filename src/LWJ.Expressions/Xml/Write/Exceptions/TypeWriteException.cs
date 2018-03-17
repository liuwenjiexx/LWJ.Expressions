/**************************************************************
 *  Filename:    TypeWriteException.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/
using System;

namespace LWJ.Expressions.Xml
{
    public class TypeWriteException : ExpressionWriteException
    {
        public TypeWriteException(string message, Expression expr, Type type)
            : this(message, expr, type, null)
        {
        }

        public TypeWriteException(string message, Expression expr, Type type, Exception innerException)
            : base(message ?? Resource1.Write_TypeError, expr, innerException)
        {
            this.Type = type;
        }

        public Type Type { get; private set; }

        public override string Message
        {
            get
            {
                string message = base.Message;
                if (Type != null)
                    message += Environment.NewLine + Resource1.Write_Type.FormatArgs(Type.FullName);
                return message;
            }
        }

    }
}
