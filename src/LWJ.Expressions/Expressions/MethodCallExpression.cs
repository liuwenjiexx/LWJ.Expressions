/**************************************************************
 *  Filename:    MethodCallExpression.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace LWJ.Expressions
{
    public class MethodCallExpression : Expression
    {
        private Expression instance;
        private MethodInfo method;
        private ReadOnlyCollection<Expression> arguments;


        internal MethodCallExpression(Expression instance, MethodInfo method, IEnumerable<Expression> arguments)
            : base(ExpressionType.Call, method.ReturnType)
        {
            this.instance = instance;
            this.method = method ?? throw new ArgumentNullException(nameof(method));
            this.arguments = new ReadOnlyCollection<Expression>(arguments.ToArray());

        }

        public Expression Instance { get => instance; }

        public ReadOnlyCollection<Expression> Arguments { get => arguments; }

        public MethodInfo Method => method;

        public override CompiledDelegate Compile(CompileContext ctx)
        {

            var mInfo = this.method;
            var argumentEvals = this.arguments.Select(o => o.Compile(ctx)).ToArray();
            int argLength = argumentEvals.Length;


            if (mInfo.IsStatic)
            {
                return (invoke) =>
                {
                    var args = new object[argLength];
                    for (int i = 0; i < argLength; i++)
                        args[i] = argumentEvals[i](invoke);
                    return mInfo.Invoke(null, args);
                };
            }

            var instanceEval = this.instance.Compile(ctx);
            return (invoke) =>
            {
                var args = new object[argLength];
                for (int i = 0; i < argLength; i++)
                    args[i] = argumentEvals[i](invoke);
                return mInfo.Invoke(instanceEval(invoke), args);
            };
        }

    }
}
