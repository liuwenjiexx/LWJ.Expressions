/**************************************************************
 *  Filename:    TypeUnaryExpression.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/
using System;
using System.Reflection;

namespace LWJ.Expressions
{
    public class TypeUnaryExpression : UnaryExpression
    {

        private Type typeOperand;

        internal TypeUnaryExpression(ExpressionType expressionType, Expression operand, Type typeOperand, MethodInfo method)
            : base(expressionType, operand, method)
        {
            this.typeOperand = typeOperand;
        }

        public override Type ValueType
        {
            get
            {
                return typeOperand;
            }
        }

        public Type TypeOperand => typeOperand;

        //OperatorInfo OperInfo => CompileContext.Current.GetOperatorInfo(ExpressionType, typeof(object), typeof(Type));

        public override CompiledDelegate Compile(CompileContext ctx)
        {
            var typeOperand = this.typeOperand;
            var lhs = Operand.Compile(ctx);

            var method = this.Method;
            if (ExpressionType == ExpressionType.Convert && method.GetParameters().Length == 2)
            {
                return (invoke) => System.Convert.ChangeType(lhs(invoke), typeOperand);
            }
            return (invoke) => method.Invoke(null, new object[] { lhs(invoke) });
        }

    }
}
