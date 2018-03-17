using LWJ.Expressions;
using static LWJ.Expressions.Expression;
using System.Collections;
using System.Collections.Generic;

namespace Test.LWJ.Expressions
{
    public class TestBase
    {
        public static T Eval<T>(Expression expression, ExpressionContext ctx = null)
        {
            return (T)Eval(null, expression, ctx);
        }
        public static T Eval<T>(Dictionary<string, object> variables, Expression expression, ExpressionContext ctx = null)
        {
            return (T)Eval(variables, expression, ctx);
        }

        public static object Eval(Expression expression, ExpressionContext ctx = null)
        {
            return Eval(null, expression, ctx);
        }

        public static object Eval(Dictionary<string, object> variables, Expression expression, ExpressionContext ctx = null)
        {
            if (ctx == null)
                ctx = new ExpressionContext();


            var compile = new CompileContext();
            InvocationDelegate eval;

            if (variables != null)
            {
                foreach (var item in variables)
                {
                    compile.AddVariable(item.Value.GetType(), item.Key);
                    ctx.AddVariable(item.Key, item.Value);
                }
            }
             
                eval = compile.Compile(expression);
              
            return eval(ctx);
        }

    }
}
