using System;

namespace LWJ.Expressions
{
    public class GotoExpression : Expression
    {
        private Expression value;
        private GotoType gotoType;


        internal GotoExpression(GotoType gotoType, Expression value)
            : base(ExpressionType.Goto)
        {
            this.gotoType = gotoType;
            this.value = value;
        }

        public GotoType GotoType => gotoType;

        public Expression Value => value;

        public override Type ValueType => value.ValueType;

        public override CompiledDelegate Compile(CompileContext ctx)
        {
            var gotoType = this.gotoType;

            if (gotoType == GotoType.Return)
            {
                var valueEval = value.Compile(ctx);
                return (invoke) =>
                {
                    object returnValue = valueEval(invoke);
                    invoke.SetGoto(GotoType.Return, returnValue);
                    return returnValue;
                };
            }
            else
            {
                return (invoke) =>
                {
                    invoke.SetGoto(gotoType, null);
                    return null;
                };
            }
        }
    }
}
