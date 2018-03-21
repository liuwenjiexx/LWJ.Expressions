using LWJ.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static LWJ.Expressions.Expression;
using System.Collections.Generic;

namespace Test.LWJ.Expressions
{

    [TestClass]
    public class LoopTest : TestBase
    {

        [TestMethod]
        public void Loop_1()
        {
            LoopExpression loop;
            var varI = Variable<int>("i");
            loop = Loop(Block(
                If(GreaterThanOrEqual(varI, Constant(1)), Break()),
                Assign(varI, Add(varI, Constant(1)))
                ));

            var ctx = new ExpressionContext();

            Eval(new Dictionary<string, object>() { { "i", 0 } }, loop, ctx);

            Assert.AreEqual(1, ctx["i"]);
        }

        [TestMethod]
        public void Loop_5()
        {
            LoopExpression loop;
            var varI = Variable<int>("i");
            loop = Loop(Block(
                If(GreaterThanOrEqual(varI, Constant(5)), Break()),
                  PreIncrement(varI)
                ));

            var ctx = new ExpressionContext();

            Eval(new Dictionary<string, object>() { { "i", 0 } }, loop, ctx);

            Assert.AreEqual(5, ctx["i"]);
        }
        [TestMethod]
        public void Loop_LocalVar_5()
        {

            var varI = Variable<int>("i");
            Expression loop = Block(
                new ParameterExpression[]
                {
                    varI
                },
                Loop(
                    Block(
                        If(GreaterThanOrEqual(varI, Constant(5)), Break()),
                        Assign(varI, Add(varI, Constant(1))),
                        Assign(Variable<int>("n"), varI)
                )));

            var ctx = new ExpressionContext();

            Eval(new Dictionary<string, object>() { { "n", 0 } }, loop, ctx);

            Assert.AreEqual(5, ctx["n"]);
        }

        [TestMethod]
        public void Loop_1000()
        {
            int count = 10000;
            var varI = Variable<int>("i");
            Expression loop = Loop(
                Block(
                    If(GreaterThanOrEqual(varI, Constant(count)), Break()),
                    Assign(varI, Add(varI, Constant(1)))
                ));

            var ctx = new ExpressionContext();

            UnitTest1.DebugTime(string.Format("expression loop {0} count time(ms): ", count), () => Eval(new Dictionary<string, object>() { { "i", 1 } }, loop, ctx));
            Assert.AreEqual(count, ctx["i"]);
            ctx["i"] = 0;
            UnitTest1.DebugTime(string.Format("loop {0} count time(ms): ", count), () =>
            {
                for (int i = 0; i < count; i++)
                {
                    ctx.SetVariable("i", (int)ctx.GetVariable("i") + 1);
                }
            });
            Assert.AreEqual(count, ctx["i"]);
            ctx["i"] = 0;
            UnitTest1.DebugTime(string.Format("local variable loop {0} count time(ms): ", count), () =>
            {
                var ctx1 = ctx;
                for (int i = 0, len = count; i < len; i++)
                {
                    ctx1.SetVariable("i", (int)ctx1.GetVariable("i") + 1);
                }
            });
            Assert.AreEqual(count, ctx["i"]);
        }




    }
}
