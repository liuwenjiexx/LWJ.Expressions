using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LWJ.Expressions.Xml.Test
{
    [TestClass]
    public class CallTest : TestBase
    {
        [TestMethod]
        public void Call_()
        {

            var dt = DateTime.Parse("1/2/2017 03:04:05");
            var sss = dt.ToString();
            ExpressionContext ctx = new ExpressionContext();
            ctx.AddVariable(typeof(TestClass), "instance", new TestClass());
            CompileContext compile = new CompileContext();
            compile.AddVariable(typeof(TestClass), "instance");
            Run(Resource1.call, compile, ctx);
        }

        [TestMethod]
        public void Call2_()
        {
            try
            {
                Run(Resource1.call);
                Assert.Fail();
            }
            catch (VariableException ex)
            {
            }
        }

        public class TestClass
        {
            public static int staticMin(int a, int b)
            {
                if (a < b)
                    return a;
                return b;
            }
            public int instanceMin(int a, int b)
            {
                if (a < b)
                    return a;
                return b;
            }
        }

    }
}
