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


            Assert.AreEqual(Eval(Or(True, Constant(1))), true);
            Assert.AreEqual(Eval(Or(False, Constant(1))), 1);

        }
        [TestMethod]
        public void And_()
        {
            Assert.IsTrue(Eval<bool>(And(True, True)));
            Assert.IsFalse(Eval<bool>(And(True, False)));
            Assert.IsFalse(Eval<bool>(And(False, False)));

            Assert.AreEqual(Eval(And(True, Constant(1))), 1);
            Assert.AreEqual(Eval(And(False, Constant(1))), false);
        }

        [TestMethod]
        public void Not_()
        {
            Assert.IsFalse(Eval<bool>(Not(True)));
            Assert.IsTrue(Eval<bool>(Not(False)));
        }
        [TestMethod]
        public void If()
        {
            Assert.AreEqual(Eval(Expression.If(True, Constant(1))), 1);
            Assert.AreEqual(Eval(Expression.If(False, Constant(1))), null);
            Assert.AreEqual(Eval(Expression.If(And(True, Constant(1)), Constant(2))), 2);
            Assert.AreEqual(Eval(Expression.If(And(False, Constant(1)), Constant(2))), null);
        }
    }
}
