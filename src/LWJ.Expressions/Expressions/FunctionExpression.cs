using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace LWJ.Expressions
{
    public class FunctionExpression : Expression
    {
        private string name;
        private Type returnType;
        private Expression body;
        private ReadOnlyCollection<ParameterExpression> arguments;
        private Type functionType;

        internal FunctionExpression(string name, IEnumerable<ParameterExpression> arguments, Expression body, Type returnType)
            : base(ExpressionType.Function)
        {
            this.name = name;
            if (arguments == null)
                this.arguments = ReadOnlyEmptyVariables;
            else
                this.arguments = new ReadOnlyCollection<ParameterExpression>(arguments.ToArray());

            if (body == null)
                this.body = Null;
            else
                this.body = body;

            this.returnType = returnType;

            if (returnType == typeof(void))
                functionType = GetActionType(this.arguments.Select(o => o.ValueType).ToArray());
            else
                functionType = GetFuncType(this.arguments.Select(o => o.ValueType).Union(new Type[] { returnType }).ToArray());

            //if (returnType == typeof(void))
            //    functionType = GetActionType(this.arguments.Select(o => typeof(object)).ToArray());
            //else
            //    functionType = GetFuncType(this.arguments.Select(o => typeof(object)).Union(new Type[] { typeof(object) }).ToArray());

        }

        public string Name => name;

        public ReadOnlyCollection<ParameterExpression> Arguments => arguments;

        public Expression Body => body;

        public Type ReturnType => returnType;

        public Type FunctionType => functionType;

        public override Type ValueType => returnType;


        public override CompiledDelegate Compile(CompileContext ctx)
        {
            var parent = CompileContext.Current;

            var func = CompileFunction(ctx);

            //if (!string.IsNullOrEmpty(name))
            //{
            //    parent.AddVariable(ValueType, name);

            //}
            return (invoke) =>
            {
                FunctionInvoker funcInvoke = new FunctionInvoker(invoke);

                var del = func(funcInvoke);
                if (!string.IsNullOrEmpty(name))
                    ((ExpressionContext)invoke.Context).AddVariable<Delegate>(name, del);
                return del;
            };
        }

        class FunctionInvoker
        {
            public InvocationContext invoke;
            public CompiledDelegate body;
            public ReadOnlyCollection<ParameterExpression> arguments;
            public FunctionInvoker(InvocationContext invoke)
            {
                this.invoke = invoke;
            }
        }

        private Func<FunctionInvoker, Delegate> CompileFunction(CompileContext ctx)
        {

            if (!string.IsNullOrEmpty(name))
                ctx.AddVariable(ValueType, name);

            CompiledDelegate evalBody;

            var arguments = this.arguments;
            var child = new CompileContext(ctx, BlockType.Method);
            foreach (var arg in arguments)
            {
                child.AddVariable(arg.ValueType, arg.Name);
            }
            evalBody = body.Compile(child);

            return (funcInvoke) =>
            {
                funcInvoke.body = evalBody;
                funcInvoke.arguments = arguments;

                switch (arguments.Count)
                {
                    case 0: return Func0(funcInvoke);
                    case 1: return Func1(funcInvoke);
                    case 2: return Func2(funcInvoke);
                    case 3: return Func3(funcInvoke);
                    case 4: return Func4(funcInvoke);
                }
                return null;
            };
        }


        Func<object> Func0(FunctionInvoker funcInvoke)
        {
            return () =>
            {
                var child = funcInvoke.invoke.CreateChild(BlockType.Method);
                funcInvoke.body(child);
                return child.GotoValue;
            };
        }

        Func<object, object> Func1(FunctionInvoker funcInvoke)
        {
            return (arg1) =>
            {
                var param1 = funcInvoke.arguments[0];
                var child = funcInvoke.invoke.CreateChild(BlockType.Method);
                var exprCtx = ((ExpressionContext)child.Context);
                exprCtx.AddVariable(param1.ValueType, param1.Name, arg1);
                funcInvoke.body(child);

                return child.GotoValue;
            };
        }
        Func<object, object, object> Func2(FunctionInvoker funcInvoke)
        {
            return (arg1, arg2) =>
            {
                var ps = funcInvoke.arguments;
                var param1 = ps[0];
                var param2 = ps[1];
                var child = funcInvoke.invoke.CreateChild(BlockType.Method);
                var exprCtx = ((ExpressionContext)child.Context);
                exprCtx.AddVariable(param1.ValueType, param1.Name, arg1);
                exprCtx.AddVariable(param2.ValueType, param2.Name, arg2);
                funcInvoke.body(child);

                return child.GotoValue;
            };
        }

        Func<object, object, object, object> Func3(FunctionInvoker funcInvoke)
        {
            return (arg1, arg2, arg3) =>
            {
                var ps = funcInvoke.arguments;
                var param1 = ps[0];
                var param2 = ps[1];
                var param3 = ps[2];
                var child = funcInvoke.invoke.CreateChild(BlockType.Method);
                var exprCtx = ((ExpressionContext)child.Context);
                exprCtx.AddVariable(param1.ValueType, param1.Name, arg1);
                exprCtx.AddVariable(param2.ValueType, param2.Name, arg2);
                exprCtx.AddVariable(param3.ValueType, param3.Name, arg3);

                funcInvoke.body(child);

                return child.GotoValue;
            };
        }
        Func<object, object, object, object, object> Func4(FunctionInvoker funcInvoke)
        {
            return (arg1, arg2, arg3, arg4) =>
            {
                var ps = funcInvoke.arguments;
                var param1 = ps[0];
                var param2 = ps[1];
                var param3 = ps[2];
                var param4 = ps[3];
                var child = funcInvoke.invoke.CreateChild(BlockType.Method);
                var exprCtx = ((ExpressionContext)child.Context);
                exprCtx.AddVariable(param1.ValueType, param1.Name, arg1);
                exprCtx.AddVariable(param2.ValueType, param2.Name, arg2);
                exprCtx.AddVariable(param3.ValueType, param3.Name, arg3);
                exprCtx.AddVariable(param3.ValueType, param4.Name, arg4);
                funcInvoke.body(child);
                return child.GotoValue;
            };
        }


        public override string ToString()
        {
            return "funcation: name={0}".FormatArgs(name);
        }

    }

}
