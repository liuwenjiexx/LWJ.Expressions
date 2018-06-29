/**************************************************************
 *  Filename:    ConditionalExpression.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/
using System;

namespace LWJ.Expressions
{

    /// <summary>
    /// bool test ? true expr : false expr
    /// </summary>
    public class ConditionalExpression : Expression
    {
        private Expression ifTrue;
        private Expression ifFalse;
        private Expression test;


        internal ConditionalExpression(Expression test, Expression ifTrue, Expression ifFalse)
            : base(ExpressionType.Conditional)
        {
            this.test = test;
            this.ifTrue = ifTrue;
            this.ifFalse = ifFalse;
        }

        public Expression IfTrue => ifTrue;
        public Expression IfFalse => ifFalse;
        public Expression Test => test;


        public override Type ValueType
        {
            get
            {
                Type valueType;
                if (ifTrue != null && ifFalse != null)
                {
                    if (ifTrue.ValueType != ifFalse.ValueType)
                        throw new ExpressionException("{0} true and false return type not equal".FormatArgs(nameof(ConditionalExpression)));
                    return ifTrue.ValueType;
                }
                else if (ifTrue != null)
                    valueType = ifTrue.ValueType;
                else if (ifFalse != null)
                    valueType = ifFalse.ValueType;
                else
                    valueType = typeof(void);

                return valueType;
            }
        }

        public override CompiledDelegate Compile(CompileContext ctx)
        {
            var testEval = test.Compile(ctx);
            var ifTrueEval = ifTrue != null ? ifTrue.Compile(ctx) : null;
            var ifFalseEval = ifFalse != null ? ifFalse.Compile(ctx) : null;

            if (ifTrueEval == null && ifFalseEval == null)
            {
                return (invoke) =>
                {
                    testEval(invoke);
                    return null;
                };
            }
            else if (ifTrueEval == null)
            {
                return (invoke) => DefaultOperators.IsTrue(testEval(invoke)) ? null : ifFalseEval(invoke);
            }
            else if (ifFalseEval == null)
            {
                return (invoke) => DefaultOperators.IsTrue(testEval(invoke)) ? ifTrueEval(invoke) : null;
            }
            else
            {
                return (invoke) => DefaultOperators.IsTrue(testEval(invoke)) ? ifTrueEval(invoke) : ifFalseEval(invoke);
            }

        }



    }
}
