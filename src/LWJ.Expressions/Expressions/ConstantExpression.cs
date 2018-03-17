/**************************************************************
 *  Filename:    ConstantExpression.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/
using System;

namespace LWJ.Expressions
{
    /// <summary>
    /// constant type: int, long, float, double, bool, string, DateTime, Type
    /// </summary>
    public class ConstantExpression : Expression
    {
        private object value;
         

        internal ConstantExpression(object value, Type valueType)
            : base(ExpressionType.Constant, valueType)
        {
            this.value = value; 
        }

        public object Value => value;


        public override CompiledDelegate Compile(CompileContext ctx)
        {            
            var value = this.value;
            return (invoke) => value;
        }
          

    }

}
