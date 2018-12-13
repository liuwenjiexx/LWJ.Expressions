using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWJ.Expressions
{
    public class GroupExpression : UnaryExpression
    {
        public GroupExpression(Expression operand)
            : base(ExpressionType.Group, operand, null)
        {

        }

        public override Type ValueType => Operand.ValueType;

        public override CompiledDelegate Compile(CompileContext ctx)
        {
            return Operand.Compile(ctx);
        }


    }
}
