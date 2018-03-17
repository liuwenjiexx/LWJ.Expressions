using LWJ.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static LWJ.Expressions.Expression;

namespace Test.LWJ.Expressions
{


    namespace Test.LWJ.Expressions
    {
        [TestClass]
        public class CalculateTest : TestBase
        {

            /// <summary>
            /// result= n+(n-1)+(n-2)...+1
            /// 
            /// </summary>
            [TestMethod]
            public void Sum()
            {
                int a = 3;
                var n = Variable<int>("n");
                var funcExpr = Function("sum", new ParameterExpression[] { n },
                        Return(
                            IfThenElse(LessThanOrEqual(n, Constant(1)),
                            Constant(1),
                            Add(n, Call("sum", n.ValueType, PostDecrement(n))))
                            )
                    , typeof(int));
                var compile = new CompileContext();
                //using (compile.BeginScope())
                //{
                var func = compile.Compile(Call(funcExpr, n.ValueType, Constant(a)));
                var result2 = func(null);
                int result = RecursiveSum(a);
                Assert.AreEqual(result, result2);
                //}
            }

            static int RecursiveSum(int n)
            {
                return n <= 1 ? 1 : n + RecursiveSum(--n);
            }
        }

    }
}
