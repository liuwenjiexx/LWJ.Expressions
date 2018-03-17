using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static LWJ.Expressions.Expression;
using LWJ.Expressions;

namespace Test.LWJ.Expressions
{
    [TestClass]
    public class CallTest : TestBase
    {

        [TestMethod]
        public void Call_InstanceMethod()
        {
            var varObj = Variable<CallClass>("obj");
            var expr = Call(varObj, "Hello");
            var compile = new CompileContext();
            var ctx = new ExpressionContext();
            ctx.AddVariable<CallClass>("obj", new CallClass());

            var result = compile.Compile(expr)(ctx);
            Assert.AreEqual("Instance: Hello World", result);
        }

        [TestMethod]
        public void Call_StaticMethod()
        {

            var expr = Call(typeof(CallClass), "StaticHello");
            var compile = new CompileContext();
            var ctx = new ExpressionContext();


            var result = compile.Compile(expr)(ctx);
            Assert.AreEqual("Static: Hello World", result);
        }

        [TestMethod]
        public void Call_Delegate()
        {
            var varObj = Variable<string>("del");
            var expr = Call(varObj, typeof(string));
            var compile = new CompileContext();
            var ctx = new ExpressionContext();
            ctx.AddVariable("del", new Func<string>(() =>
            {
                return "Hello World";
            }));

            var result = compile.Compile(expr)(ctx);
            Assert.AreEqual("Hello World", result);

        }
        [TestMethod]
        public void Call_DelegateArgs1()
        {
            var varObj = Variable<string>("del");
            var expr = Call(varObj, typeof(string),Constant("My"));
            var compile = new CompileContext();
            var ctx = new ExpressionContext();
            ctx.AddVariable("del", new Func<string, string>((say) =>
            {
                return say + ": Hello World";
            }));

            var result = compile.Compile(expr)(ctx);
            Assert.AreEqual("My: Hello World", result);

        }

        class CallClass
        {
            public string Hello()
            {
                return "Instance: Hello World";
            }

            public static string StaticHello()
            {
                return "Static: Hello World";
            }

        }

    }
}
