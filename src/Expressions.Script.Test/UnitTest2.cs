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

            CheckExpr("sub(1,2)", -1D, ctx);
            CheckExpr("sub3(1,2,3)", -4D, ctx);
            CheckExpr("max(1,2)", 2D, ctx);
            CheckExpr("max(-1,-2)", -1D, ctx);

            CheckExpr("PI", Math.PI, ctx);
            CheckExpr("PI>1", true, ctx);

            CheckExpr("min(max(5,1),10)", 5D, ctx);
            CheckExpr("min(10,max(5,1))", 5D, ctx);

            CheckExpr("min(10,max(-5,-10))", -5D, ctx);

        }

        [TestMethod]
        public void Params()
        {
            ExpressionContext ctx = new ExpressionContext();
            AddMember(ctx, typeof(Math));

            CheckExpr("max(1,2)", 2D, ctx);
            CheckExpr("max2(1,2)", 2D, ctx);
            CheckExpr("max2(1,2,3)", 3D, ctx);

            CheckExpr("max3(1)", 1D, ctx);
            CheckExpr("max3(1,2)", 2D, ctx);

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

        [TestMethod]
        public void TestInstanceFunc()
        {
            object result;
            MyObj obj = new MyObj(1);
            ExpressionContext ctx = new ExpressionContext();
            ctx.AddVariable<Func<long, long>>("add");
            ctx.SetVariable("add", new Func<long, long>(obj.Add));
            CompileContext comple = new CompileContext(ctx);
            var expr = ScriptExpressionReader.Instance.Parse("add(2)", comple);

            result = comple.Compile(expr)(ctx);
            Assert.AreEqual(result, 3L);
        }
        [TestMethod]
        public void TestInstanceFuncParams()
        {
            object result;
            MyObj obj = new MyObj(1);
            ExpressionContext ctx = new ExpressionContext();
            ctx.AddVariable<Func<long, long[], long>>("sum");
            ctx.SetVariable("sum", new Func<long, long[], long>(obj.Sum));
            CompileContext comple = new CompileContext(ctx);

            var expr = ScriptExpressionReader.Instance.Parse("sum(2)", comple);
            result = comple.Compile(expr)(ctx);
            Assert.AreEqual(result, 3L);
            expr = ScriptExpressionReader.Instance.Parse("sum(2,3)", comple);
            result = comple.Compile(expr)(ctx);
            Assert.AreEqual(result, 6L);
            expr = ScriptExpressionReader.Instance.Parse("sum(2,3,4)", comple);
            result = comple.Compile(expr)(ctx);
            Assert.AreEqual(result, 10L);
        }
        class MyObj
        {
            private int n;
            public MyObj(int n)
            {
                this.n = n;
            }
            public long Add(long a)
            {
                return a + n;
            }

            public long Sum(long a, params long[] items)
            {
                long n1 = n;
                n1 += a;
                foreach (var b in items)
                {
                    n1 += b;
                }
                return n1;
            }

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
        public static double sub(double a, double b)
        {
            return a - b;
        }
        [ExportMember]
        public static double sub3(double a, double b, double c)
        {
            return a - b - c;
        }
        [ExportMember]
        public static double max2(double a, params double[] b)
        {
            var ret = a;
            foreach (var item in b)
                if (item > ret)
                    ret = item;
            return ret;
        }
        [ExportMember]
        public static double max3(params double[] b)
        {
            var ret = b[0];
            foreach (var item in b)
                if (item > ret)
                    ret = item;
            return ret;
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
