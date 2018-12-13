using System;

namespace LWJ.Expressions
{

    public class TypeBinaryExpresion : Expression
    {
        private Expression operand;
        private Type typeOperand;

        internal TypeBinaryExpresion(ExpressionType expressionType, Expression operand, Type operandType)
            : base(expressionType)
        {
            this.typeOperand = operandType;
            this.operand = operand;
        }

        public override Type ValueType
        {
            get
            {
                switch (ExpressionType)
                {
                    case ExpressionType.TypeIs:
                        return typeof(bool);
                    default:
                        return typeOperand;
                }
            }
        }


        public Expression Operand => operand;

        public Type TypeOperand => typeOperand;

        OperatorInfo OperInfo => CompileContext.Current.GetOperatorInfo(ExpressionType, typeof(object), typeof(Type));

        public override CompiledDelegate Compile(CompileContext ctx)
        {

            var lhs = operand.Compile(ctx);
            var typeOperand = this.typeOperand;

            var oper = OperInfo;
            var method = oper.method;
            switch (ExpressionType)
            {
                case ExpressionType.TypeAs:
                    return (invoke) => DefaultOperators.TypeAs(lhs(invoke), typeOperand);
                case ExpressionType.TypeIs:
                    return (invoke) => DefaultOperators.TypeIs(lhs(invoke), typeOperand);
            }
            throw new NotImplementedException();
        }

    }
}
