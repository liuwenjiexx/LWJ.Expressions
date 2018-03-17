using LWJ.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static LWJ.Expressions.Expression;
using System.Collections.Generic;


namespace Test.LWJ.Expressions
{
    [TestClass]
    public class VariableTest : TestBase
    {


        [TestMethod]
        public void Context_Variable_Type()
        {
            TestVariable("int1", 1);
            TestVariable("float1", 1.1f);
            TestVariable("double1", 1.1d);
            TestVariable("string1", "string value");

        }

        private void TestVariable(string name, object value)
        {
            var ctx = new ExpressionContext();

            ctx.AddVariable(value.GetType(), name, value);
            Assert.IsTrue(ctx.ContainsVariable(name));
            Assert.IsFalse(ctx.ContainsVariable("_" + name));
            Assert.AreEqual(value, ctx.GetVariable(name));
        }

        [TestMethod]
        public void Variable_Set()
        {
            var ctx = new ExpressionContext();
            var eval = Eval(new Dictionary<string, object>() { { "int1", 0 } },
                 Assign(Variable<int>("int1"), Constant(2)),
                 ctx);

            Assert.AreEqual(2, ctx["int1"]);
        }

        [TestMethod]
        public void Variable_Get()
        {
            var ctx = new ExpressionContext();
            var result = Eval(
                new Dictionary<string, object>() { { "int1", 1 } },
                 Variable<int>("int1"),
                 ctx);
            Assert.AreEqual(1, result);
        }

    }
}
