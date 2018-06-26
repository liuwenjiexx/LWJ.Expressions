﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LWJ.Expressions.Script;
using System.Reflection;

namespace LWJ.Expressions.Script.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Number()
        {
            CheckExpr("1", 1L);
            CheckExpr("-1", -1L);
            CheckExpr("1.2", 1.2d);
            CheckExpr("0.1", 0.1d);
            CheckExpr(".1", 0.1d);
        }



        [TestMethod]
        public void Arithmetic()
        {
            CheckExpr("1+2", 3d);
            CheckExpr("2*3", 6L);
            CheckExpr("4/2", 2d);
            CheckExpr("3%2", 1d);
        }
        [TestMethod]
        public void Priority()
        {
            CheckExpr("1+2*3", 7d);
            CheckExpr("1*2+3", 5D);
        }

        [TestMethod]
        public void Logic()
        {
            CheckExpr("1<2", true);
            CheckExpr("2<1", false);
            CheckExpr("1<=2", true);
            CheckExpr("2<=1", false);
            CheckExpr("2>1", true);
            CheckExpr("1>2", false);
            CheckExpr("2>=1", true);
            CheckExpr("1>=2", false);
            CheckExpr("1==1", true);
            CheckExpr("1==2", false);
            CheckExpr("1!=2", true);
            CheckExpr("1!=1", false);
        }

        [TestMethod]
        public void Group()
        {
            CheckExpr("(1)", 1L);
            CheckExpr("(-1)", -1L);
            CheckExpr("(1+2)", 3D);
            CheckExpr("(1+2)*3", 9D);
            CheckExpr("1+(2*3)", 7D);
            CheckExpr("3*(1+2)", 9D);
        }


        [TestMethod]
        public void Member()
        {


            ExpressionContext ctx = new ExpressionContext();
            ctx.AddVariable<MyClass>("obj");
            ctx.SetVariable("obj", new MyClass()
            {
                IntField = 1,
                IntProperty = 2,
                Next = new MyClass()
                {
                    IntField = 3,
                    IntProperty = 4,
                }
            });
            CheckExpr("obj.IntField", 1, ctx);
            CheckExpr("obj.IntProperty", 2, ctx);
            CheckExpr("obj.Next.IntField", 3, ctx);
            CheckExpr("obj.Next.IntProperty", 4, ctx);
        }
        [TestMethod]
        public void Call()
        {
            ExpressionContext ctx = new ExpressionContext();
            ctx.AddVariable<MyClass>("obj");
            ctx.SetVariable("obj", new MyClass()
            {
            });
            CheckExpr("obj.Negate(1)", -1L, ctx);
            CheckExpr("obj.Add(1,2)", 3L, ctx);
            CheckExpr("obj.Add_ABC(1,2,3)", 6L, ctx);
            //  CheckExpr("obj.Sum_Params(1,2,3)", 6L, ctx);
            CheckExpr("obj.Add(1,2)+obj.Add(3,4)", 10D, ctx);
        }

        [TestMethod]
        public void Call_Static()
        {
            ExpressionContext ctx = new ExpressionContext();
            ctx.AddVariable<MethodInfo>("Static_Add");
            var mInfo = typeof(MyClass).GetMethod("Static_Add", BindingFlags.Public | BindingFlags.Static);
         
            ctx.SetVariable("Static_Add", mInfo);
            CheckExpr("Static_Add(1,2)", 3L, ctx);

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
            Assert.AreEqual(expectedResult, result, exprString);
        }

        class MyClass
        {
            public int IntField;
            public int IntProperty { get; set; }

            public MyClass Next { get; set; }

            public long Add(long a, long b)
            {
                return a + b;
            }
            public long Add_ABC(long a, long b, long c)
            {
                return a + b + c;
            }
            public long Negate(long a)
            {
                return -a;
            }
            public static long Static_Add(long a, long b)
            {
                return a + b;
            }
            public long Sum_Params(params long[] values)
            {
                long n = 0;
                foreach (var v in values)
                {
                    n += v;
                }
                return n;
            }
        }

    }

}