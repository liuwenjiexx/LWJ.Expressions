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
            return _Compile(ctx, value);
        }

        private static CompiledDelegate _Compile(CompileContext ctx, object value)
        {
            return (invoke) =>
            {
                return value;
            };
        }

    }

}
