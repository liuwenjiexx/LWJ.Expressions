/**************************************************************
 *  Filename:    FunctionCallExpression.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LWJ.Expressions
{
    public class FunctionCallExpression : Expression
    {
        private Expression function;
        private ReadOnlyCollection<Expression> arguments;

        internal FunctionCallExpression(Expression function, Type returnType, IEnumerable<Expression> arguments)
            : base(ExpressionType.Call, returnType)
        {
            this.function = function;
            if (arguments == null)
                this.arguments = ReadOnlyEmptyExpressions;
            else
                this.arguments = new ReadOnlyCollection<Expression>(arguments.ToArray());
        }

        public new Expression Function { get => function; }

        public ReadOnlyCollection<Expression> Arguments { get => arguments; }

        public override CompiledDelegate Compile(CompileContext ctx)
        {
            
            var funcEval = function.Compile(ctx);
            var evalArgs = arguments.Select(o => o.Compile(ctx)).ToArray();
            return (invoke) =>
            {
                var func = (Delegate)funcEval(invoke);
                var args = evalArgs.Select(o => o(invoke)).ToArray();
                if (func.Method.IsParamArray())
                {
                    args = func.Method.AlignParamArrayArguments(args);
                }
                return func.DynamicInvoke(args);
            };
        }

        public override string ToString()
        {
            return "call func: {0}".FormatArgs(function);
        }


    }
}
