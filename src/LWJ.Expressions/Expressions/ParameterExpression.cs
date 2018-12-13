using System;

namespace LWJ.Expressions
{
    public class ParameterExpression : AccessableExpression
    {
        private string name;
        private Type type;
        private Expression defaultValue;

        internal ParameterExpression(Type type, string name, Expression defaultValue = null)
            : base(ExpressionType.Variable, type)
        {
            this.type = type;
            this.name = name;
            this.defaultValue = defaultValue;
        }

        public string Name => name;



        public override CompiledDelegate Compile(CompileContext ctx)
        {
            string name = this.name;
            return (invoke) => invoke.GetVariable(name);
        }


        public override Action<InvocationContext, object> CompileSetValue(CompileContext ctx)
        {
            string name = this.name;
            return (invoke, value) =>
            {
                invoke.SetVariable(name, value);
            };
        }

        public Action<InvocationContext> CompileInitValue(CompileContext ctx)
        {

            Type valueType = this.type;
            string name = this.name;
            if (defaultValue == null)
                return (invoke) => ((ExpressionContext)invoke.Context).AddVariable(valueType, name);

            var valueEval = defaultValue.Compile(ctx);
            return (invoke) => ((ExpressionContext)invoke.Context).AddVariable(valueType, name, valueEval(invoke));
        }

        public override string ToString()
        {
            return "var name={0}".FormatArgs(name);
        }

    }
}
