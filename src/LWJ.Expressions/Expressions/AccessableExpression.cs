using System;

namespace LWJ.Expressions
{
    /// <summary>
    /// <see cref="VariableException"/>, <see cref="MemberExpression"/>
    /// </summary>
    public abstract class AccessableExpression : Expression
    {
        protected AccessableExpression()
        {
        }

        protected AccessableExpression(ExpressionType expressionType, Type valueType)
            : base(expressionType, valueType)
        {
        }

        public abstract Action<InvocationContext, object> CompileSetValue(CompileContext ctx);
    }



}
