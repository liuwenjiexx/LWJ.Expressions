/**************************************************************
 *  Filename:    AddAssignExpression.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/
using System;

namespace LWJ.Expressions
{
    internal class AddAssignExpression : UnaryExpression
    {

        private Expression value;
        internal AddAssignExpression(ExpressionType expressionType, AccessableExpression left, Expression value)
          : base(expressionType, left, null)
        {
            this.value = value;
        }

        public override Type ValueType => Operand.ValueType;

        public override CompiledDelegate Compile(CompileContext ctx)
        {
            var valueEval = Add((AccessableExpression)this.Operand, this.value).Compile(ctx);
            var setterEval = ((AccessableExpression)Operand).CompileSetValue(ctx);

            if (ExpressionType == ExpressionType.PreIncrement ||
                ExpressionType == ExpressionType.PreDecrement)
            {
                var getterEval = ((AccessableExpression)Operand).Compile(ctx);
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
