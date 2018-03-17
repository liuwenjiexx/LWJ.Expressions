/**************************************************************
 *  Filename:    OperatorNotImplementedException.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/
using System;

namespace LWJ.Expressions
{
    public class OperatorNotImplementedException : NotImplementedException
    {
        public OperatorNotImplementedException(ExpressionType @operator, Type operType1, Type operType2)
            : this(@operator, operType1, operType2, Resource1.OperNotImpl)
        {
        }
        
        public OperatorNotImplementedException(ExpressionType @operator, Type operType1, Type operType2, string message)
            : base(message)
        {
            this.Operator = @operator;
            this.OperType1 = operType1;
            this.OperType2 = operType2;
        }
        public ExpressionType Operator { get; private set; }

        public Type OperType1 { get; private set; }

        public Type OperType2 { get; private set; }

        public override string Message
        {
            get
            {
                string message = base.Message;

                message += Environment.NewLine + string.Format("Operator : {0}", Operator);

                if (OperType1 != null)
                    message += Environment.NewLine + string.Format("Type 1: {0}", OperType1.FullName);
                else
                    message += Environment.NewLine + "Type 1: null";
                if (OperType2 != null)
                    message += Environment.NewLine + string.Format("Type 2: {0}", OperType2.FullName);
                else
                    message += Environment.NewLine + "Type 2: null";
                return message;
            }
        }


    }
}
