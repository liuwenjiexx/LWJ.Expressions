/**************************************************************
 *  Filename:    UnaryExpression.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/
using System;
using System.Reflection;

namespace LWJ.Expressions
{

    public class UnaryExpression : Expression
    {
        private Expression operand;
        private bool isOperandCompiled;
        private MethodInfo method;


        internal protected UnaryExpression(ExpressionType expressionType, Expression operand, MethodInfo method)
            : base(expressionType, method != null ? method.ReturnType : typeof(void))
        {
            this.operand = operand ?? throw new ArgumentNullException(nameof(operand));
            this.method = method;
        }

        public Expression Operand => operand;

        public MethodInfo Method => method;

        public override CompiledDelegate Compile(CompileContext ctx)
        {

            var lhs = operand.Compile(ctx);
            if (isOperandCompiled)
                return lhs;

            var method = this.method;
            return (invoke) => method.Invoke(null, new object[] { lhs(invoke) });
        }

    }


}