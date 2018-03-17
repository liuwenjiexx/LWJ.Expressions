using LWJ.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static LWJ.Expressions.Expression;

namespace Test.LWJ.Expressions
{

    [TestClass]
    public class CompareTest : TestBase
    {

        [TestMethod]
        public void Equal_()
        {
            Assert.IsTrue(Eval<bool>(Equal(Constant(1), Constant(1))));
            Assert.IsFalse(Eval<bool>(Equal(Constant(1), Constant(2))));
            Assert.IsFalse(Eval<bool>(Equal(Constant(2), Constant(1))));
        }

        [TestMethod]
        public void NotEqual_()
        {
            Assert.IsFalse(Eval<bool>(NotEqual(Constant(1), Constant(1))));
            Assert.IsTrue(Eval<bool>(NotEqual(Constant(1), Constant(2))));
            Assert.IsTrue(Eval<bool>(NotEqual(Constant(2), Constant(1))));
        }

        [TestMethod]
        public void LessThan_()
        {
            Assert.IsFalse(Eval<bool>(LessThan(Constant(1), Constant(1))));
            Assert.IsTrue(Eval<bool>(LessThan(Constant(1), Constant(2))));
            Assert.IsFalse(Eval<bool>(LessThan(Constant(2), Constant(1))));
        }

        [TestMethod]
        public void LessThanOrEqual_()
        {
            Assert.IsTrue(Eval<bool>(LessThanOrEqual(Constant(1), Constant(1))));
            Assert.IsTrue(Eval<bool>(LessThanOrEqual(Constant(1), Constant(2))));
            Assert.IsFalse(Eval<bool>(LessThanOrEqual(Constant(2), Constant(1))));
        }

        [TestMethod]
        public void GreaterThan_()
        {
            Assert.IsFalse(Eval<bool>(GreaterThan(Constant(1), Constant(1))));
            Assert.IsFalse(Eval<bool>(GreaterThan(Constant(1), Constant(2))));
            Assert.IsTrue(Eval<bool>(GreaterThan(Constant(2), Constant(1))));
        }

        [TestMethod]
        public void GreaterThanOrEqual_()
        {
            Assert.IsTrue(Eval<bool>(GreaterThanOrEqual(Constant(1), Constant(1))));
            Assert.IsFalse(Eval<bool>(GreaterThanOrEqual(Constant(1), Constant(2))));
            Assert.IsTrue(Eval<bool>(GreaterThanOrEqual(Constant(2), Constant(1))));
        }

        [TestMethod]
        public void GreaterThanOrEqual2_()
        {
            var n = Variable(typeof(int), "n");
            Assert.IsTrue(Eval<bool>(Block(new ParameterExpression[] { n },
             Return(GreaterThanOrEqual(PostIncrement(n), Constant(1))))));

        }
    }
}
