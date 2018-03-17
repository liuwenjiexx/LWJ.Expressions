using LWJ.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static LWJ.Expressions.Expression;

namespace Test.LWJ.Expressions
{

    [TestClass]
    public class LogicTest : TestBase
    {
        [TestMethod]
        public void Or_()
        {
            Assert.IsTrue(Eval<bool>(Or(True, True)));
            Assert.IsTrue(Eval<bool>(Or(True, False)));
            Assert.IsFalse(Eval<bool>(Or(False, False)));
        }
        [TestMethod]
        public void And_()
        {
            Assert.IsTrue(Eval<bool>(And(True, True)));
            Assert.IsFalse(Eval<bool>(And(True, False)));
            Assert.IsFalse(Eval<bool>(And(False, False)));
        }
        [TestMethod]
        public void Not_()
        {
            Assert.IsFalse(Eval<bool>(Not(True)));
            Assert.IsTrue(Eval<bool>(Not(False)));
        }


    }
}
