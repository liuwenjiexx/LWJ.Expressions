using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace LWJ.Expressions.Script.Test
{
    [TestClass]
    public class ScriptMathTest
    {
        [TestMethod]
        public void MathTest()
        {
            ExpressionContext ctx = new ExpressionContext();
            AddMember(ctx, typeof(Math));

            CheckExpr("max(1,2)", 2D, ctx);
            CheckExpr("max(-1,-2)", -1D, ctx);

            CheckExpr("PI", Math.PI, ctx);
            CheckExpr("PI>1", true, ctx);

            CheckExpr("min(max(5,1),10)", 5D, ctx);
            CheckExpr("min(10,max(5,1))", 5D, ctx);

            CheckExpr("min(10,max(-5,-10))", -5D, ctx);

        }

        static void AddMember(ExpressionContext ctx, Type type)
        {
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.GetField;
            foreach (var m in type.GetMembers(bindingFlags))
            {
                var attr = m.GetCustomAttribute<ExportMemberAttribute>();
                string name = string.IsNullOrEmpty(attr.Name) ? m.Name : attr.Name;
                if (m is MethodInfo)
                {
                    ctx.AddVariable(typeof(MethodInfo), name);
                    ctx.SetVariable(name, m);
                }
                else if (m is FieldInfo)
                {
                    var fInfo = (FieldInfo)m;
                    ctx.AddVariable(fInfo.FieldType, name);
                    ctx.SetVariable(name, fInfo.GetValue(null));
                }
                else if (m is PropertyInfo)
                {
                    var pInfo = (PropertyInfo)m;
                    ctx.AddVariable(pInfo.PropertyType, name);
                    ctx.SetVariable(name, pInfo.GetGetMethod().Invoke(null, null));
                }
            }
        }
        void CheckExpr(string exprString, object expectedResult, IExpressionContext ctx = null)
        {
            Expression expr;
            var cCtx = new CompileContext(ctx);
            try
            {
                expr = ScriptExpressionReader.Instance.Parse(exprString, cCtx);
            }
            catch (Exception ex)
            {
                throw new Exception(exprString, ex);
            }
            object result;
            result = cCtx.Compile(expr)(ctx);
            Console.WriteLine("run: " + exprString + "\r\n=>: " + result);
            Assert.AreEqual(expectedResult, result, exprString);
        }

    }

    public class ExportMemberAttribute : Attribute
    {
        public ExportMemberAttribute()
        {

        }
        public ExportMemberAttribute(string name)
        {
            this.Name = name;
        }
        public string Name { get; set; }
    }

    public static class Math
    {
        [ExportMember]
        public const double PI = System.Math.PI;

        [ExportMember]
        public static double max(double a, double b)
        {
            if (a > b)
                return a;
            return b;
        }
        [ExportMember]
        public static double min(double a, double b)
        {
            if (a < b)
                return a;
            return b;
        }
        [ExportMember]
        public static double clamp(double value, double min, double max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
    }

}
