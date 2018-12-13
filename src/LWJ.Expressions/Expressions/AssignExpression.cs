using System;

namespace LWJ.Expressions
{
    public class AssignExpression : BinaryExpression
    {
        private bool isPre;
        internal AssignExpression(ExpressionType expressionType, AccessableExpression left, Expression right, bool isPre = false)
          : base(expressionType, left, right, null)
        {
            this.isPre = isPre;
        }

        internal AssignExpression(AccessableExpression left, Expression right)
            : this(ExpressionType.Assign, left, right)
        {
        }

        public override Type ValueType => Right.ValueType;



        public override CompiledDelegate Compile(CompileContext ctx)
        {
            var valueEval = Right.Compile(ctx);
            var setterEval = ((AccessableExpression)Left).CompileSetValue(ctx);
            if (isPre)
            {
                var getterEval = ((AccessableExpression)Left).Compile(ctx);
                return (invoke) =>
                {
                    var preValue = getterEval(invoke);
                    object value = valueEval(invoke);
                    setterEval(invoke, value);
                    return preValue;
                };
            }
            return (invoke) =>
            {
                object value = valueEval(invoke);
                setterEval(invoke, value);
                return value;
            };
        }

    }
}