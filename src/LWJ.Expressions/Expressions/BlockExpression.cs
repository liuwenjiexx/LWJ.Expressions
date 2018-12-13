using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace LWJ.Expressions
{

    public class BlockExpression : Expression
    {
        private ReadOnlyCollection<Expression> expressions;
        private ReadOnlyCollection<ParameterExpression> variables;


        internal BlockExpression(IEnumerable<ParameterExpression> variables, IEnumerable<Expression> expressions)
            : this(ExpressionType.Block, typeof(void), variables, expressions)
        {
        }

        internal BlockExpression(ExpressionType expressionType, Type valueType, IEnumerable<ParameterExpression> variables, IEnumerable<Expression> expressions)
            : base(expressionType, valueType)
        {
            if (variables == null)
                this.variables = ReadOnlyEmptyVariables;
            else
                this.variables = new ReadOnlyCollection<ParameterExpression>(variables.ToArray());

            if (expressions == null)
                this.expressions = ReadOnlyEmptyExpressions;
            else
                this.expressions = new ReadOnlyCollection<Expression>(expressions.ToArray());
        }

        public ReadOnlyCollection<ParameterExpression> Variables => variables;

        public ReadOnlyCollection<Expression> Expressions => expressions;



        public override CompiledDelegate Compile(CompileContext ctx)
        {

            var variables = this.variables;
            CompiledDelegate[] evals;
            Action<InvocationContext>[] evalVars;
            var child = new CompileContext(ctx, BlockType.Block);

            foreach (var variable in variables)
                child.AddVariable(variable.ValueType, variable.Name);

            evals = expressions.Select(o => o.Compile(child)).ToArray();
            evalVars = variables.Select(o => o.CompileInitValue(child)).ToArray();

            return (invoke) =>
            {
                var childInvoke = invoke.CreateChild(BlockType.Block);

                foreach (var evalVar in evalVars)
                    evalVar(childInvoke);

                foreach (var expr in evals)
                {
                    expr(childInvoke);
                    if (childInvoke.GotoType != GotoType.None)
                    {
                        childInvoke.BubblingGoto();
                        break;
                    }
                }
                return null;
            };
        }


    }


}