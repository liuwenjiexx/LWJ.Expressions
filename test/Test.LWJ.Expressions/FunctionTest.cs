using LWJ.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static LWJ.Expressions.Expression;


namespace Test.LWJ.Expressions
{
    [TestClass]
    public class FunctionTest : TestBase
    {
        [TestMethod]
        public void Delegate_Args0()
        {
            int a = 1, b = 2;
            var funcExpr = Function(null,
                Return(Add(Constant(a), Constant(b))),
                typeof(int));
            var compile = new CompileContext();
            //using (compile.BeginScope())
            //{
            var tmp = compile.Compile(funcExpr);

            var func = (Delegate)tmp(null);
            Assert.AreEqual(a + b, func.DynamicInvoke());
            //}
        }

        [TestMethod]
        public void Delegate_Args2()
        {
            int a = 1, b = 2;
            var varA = Variable<int>("a");
            var varB = Variable<int>("b");
            var funcExpr = Function(null, new ParameterExpression[] { varA, varB },
                Return(Add(varA, varB)),
                typeof(int));
            var compile = new CompileContext();
            //using (compile.BeginScope())
            //{
            var tmp = compile.Compile(funcExpr);
            var func = (Delegate)tmp(null);
            Assert.AreEqual(a + b, func.DynamicInvoke(a, b));
            //}
        }

        [TestMethod]
        public void ToFunc_Args0()
        {
            var funcExpr = Function(null,
                Return(Constant("Hello World")),
                typeof(string));
            var compile = new CompileContext();

            var tmp = compile.Compile(funcExpr);

            var del = (Delegate)tmp(null);

            //Func<string> funcString = del as Func<string>;

            Func<object> func = del as Func<object>;

            Assert.AreEqual("Hello World", func());

        }
        [TestMethod]
        public void ToFunc_Args1()
        {
            var funcExpr = Function(null, new ParameterExpression[] { Variable<string>("text") },
                Return(Variable<string>("text")),
                typeof(string));
            var compile = new CompileContext();

            var tmp = compile.Compile(funcExpr);

            var del = (Delegate)tmp(null);

            //Func<string> funcString = del as Func<string>;

            Func<object, object> func = del as Func<object, object>;

            Assert.AreEqual("Hello World", func("Hello World"));

        }

        [TestMethod]
        public void Call_NamedFunc()
        {
            int a = 1;
            var expr = Block(Function("func1", Return(Constant(a)), typeof(int)),
                Return(Call("func1", typeof(int))));
            var compile = new CompileContext();
            //using (compile.BeginScope())
            //{
            var result = compile.Compile(expr)(null);
            Assert.AreEqual(a, result);
            //}
        }

        [TestMethod]
        public void Call_Args0()
        {
            int a = 1, b = 2;
            var funcExpr = Function(null, Return(Add(Constant(a), Constant(b))), typeof(int));
            var compile = new CompileContext();
            //using (compile.BeginScope())
            //{
            var result = compile.Compile(Call(funcExpr, typeof(int)))(null);
            Assert.AreEqual(a + b, result);
            //}
        }
        [TestMethod]
        public void Call_Args2()
        {
            int a = 1, b = 2;
            var varA = Variable<int>("a");
            var varB = Variable<int>("b");
            var funcExpr = Function(null, new ParameterExpression[] { varA, varB },
                Return(Add(varA, varB)),
                typeof(int));
            var compile = new CompileContext();
            //using (compile.BeginScope())
            //{
            var result = compile.Compile(Call(funcExpr, typeof(int), Constant(a), Constant(b)))(null);
            Assert.AreEqual(a + b, result);
            //}
        }
        [TestMethod]
        public void Block_Var_Nest_Func()
        {
            var varI = Variable<int>("i");
            var expr = Block(new ParameterExpression[] { varI },
                Assign(varI, Constant(2)),
                Return(Call(Function(null, Return(varI), typeof(int)), typeof(int))));
            var compile = new CompileContext();
            //using (compile.BeginScope())
            //{
            compile.AddVariable<int>("i");
            var tmp = compile.Compile(expr);
            var ctx = new ExpressionContext();
            ctx.AddVariable<int>("i", 1);
            var result = tmp(ctx);
            Assert.AreEqual(2, result);
            //}
        }
        [TestMethod]
        public void Func_Var_Nest_Func()
        {
            var varI = Variable<int>("i");
            var funcExpr = Function("func_i", Block(new ParameterExpression[] { varI },
                Assign(varI, Constant(2)),
                Return(Call(Function(null, Return(varI), typeof(int)), typeof(int)))), typeof(int));
            var compile = new CompileContext();
            //using (compile.BeginScope())
            //{
            compile.AddVariable<int>("i");
            var tmp = compile.Compile(Call(funcExpr, typeof(int)));
            var ctx = new ExpressionContext();
            ctx.AddVariable<int>("i", 1);
            var result = tmp(ctx);
            Assert.AreEqual(2, result);
            //}

        }

        [TestMethod]
        public void Func_Clamp()
        {
            var varA = Variable<int>("a");
            var varB = Variable<int>("b");
            var expr = Block(
                Function("min_", new ParameterExpression[] { varA, varB },
                Return(IfThenElse(LessThanOrEqual(varA, varB), varA, varB)), typeof(int)),

                Function("max_", new ParameterExpression[] { varA, varB },
                Return(IfThenElse(GreaterThanOrEqual(varA, varB), varA, varB)), typeof(int)),

                Function("clamp", new ParameterExpression[] { Variable<int>("value"), Variable<int>("min"), Variable<int>("max") },
                Return(Call("min_", typeof(int), Call("max_", typeof(int), Variable<int>("value"), Variable<int>("min")), Variable<int>("max"))), typeof(int)),

                Return(Call("clamp", typeof(int), Constant(100), Constant(1), Constant(10))));
            var compile = new CompileContext();
            //using (compile.BeginScope())
            //{
            var tmp = compile.Compile(expr);
            var result = tmp(null);
            Assert.AreEqual(10, result);
            //}
        }

    }
}
