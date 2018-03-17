using LWJ.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static LWJ.Expressions.Expression;

namespace Test.LWJ.Expressions
{

    [TestClass]
    public class BlockTest : TestBase
    {
        [TestMethod]
        public void Block_NotVariable()
        {
            var int1 = Variable<int>("int1");

            var expr = Block(
                 new Expression[] {
                     Assign(int1 ,Constant(2)),
                 });

            try
            {
                var result = Eval(expr, null);
                Assert.Fail();
            }
            catch (VariableException ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        [TestMethod]
        public void Block_SetVariable()
        {
            var int1 = Variable<int>("int1");
            var int2 = Variable<int>("int2");

            var expr = Block(
                 new ParameterExpression[] {
                          int2  },
                 new Expression[] {
                     Assign(int2 ,Constant(2)),
                     Assign(int1, int2 )
                 });

            var ctx = new ExpressionContext();
            ctx.AddVariable<int>("int1", 1);

            var result = Eval(expr, ctx);
            Assert.IsTrue(ctx.ContainsVariable("int1"));
            Assert.IsFalse(ctx.ContainsVariable("int2"));
            Assert.AreEqual(2, ctx.GetVariable("int1"));
            Assert.AreEqual(null, result);

        }


        [TestMethod]
        public void Block_SetVariable2()
        {
            var ctx = new ExpressionContext();

            ctx.AddVariable<int>("int1", 1);
            var int2 = Variable<int>("int2");
            var int3 = Variable<int>("int3");
            var expr = Block(
                 new ParameterExpression[] { int2 },
                 new Expression[] {
                         Assign( int2,Constant(2)),
                         Block(new ParameterExpression[]{ int3},
                         new Expression[]{
                             Assign( int3,Constant(3)),
                             Assign(Variable<int>("int1"),Add( int2 , int3 )  )
                         })
                 });
            //System.Linq.Expressions.Expression
            var result = Eval(expr, ctx);
            Assert.IsTrue(ctx.ContainsVariable("int1"));
            Assert.IsFalse(ctx.ContainsVariable("int2"));
            Assert.IsFalse(ctx.ContainsVariable("int3"));
            Assert.AreEqual(5, ctx.GetVariable("int1"));
            Assert.AreEqual(null, result);

        }

    }
}
