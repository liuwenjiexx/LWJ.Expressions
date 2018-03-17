using LWJ.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static LWJ.Expressions.Expression;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Collections;

namespace Test.LWJ.Expressions
{
    [TestClass]
    public class ExpressionTest : TestBase
    {

        [TestMethod]
        public void Add_Int32()
        {

            ExpressionContext ctx = new ExpressionContext();

            var expr = Add(Constant(1), Constant(2));
            //System.Linq.Expressions.Expression
            Assert.AreEqual(3, Eval(expr));
        }
        [TestMethod]
        public void Add_String()
        {
            var expr = Add(Constant("hello"), Constant(" world"));

            Assert.AreEqual("hello world", Eval(expr));
        }

        [TestMethod]
        public void Subtract_Int32()
        {

            ExpressionContext ctx = new ExpressionContext();

            var expr = Subtract(Constant(1), Constant(2));

            Assert.AreEqual(-1, Eval(expr));

        }

        [TestMethod]
        public void Multiply_Int32()
        {
            ExpressionContext ctx = new ExpressionContext();
            var expr = Multiply(Constant(1), Constant(2));
            Assert.AreEqual(2, Eval(expr));



        }

        [TestMethod]
        public void Divide_Int32()
        {
            ExpressionContext ctx = new ExpressionContext();

            var expr = Divide(Constant(1), Constant(2));
            Assert.AreEqual(0, Eval(expr));
        }

        [TestMethod]
        public void Divide_Float32()
        {
            ExpressionContext ctx = new ExpressionContext();

            var expr = Divide(Constant(1f), Constant(2f));

            Assert.AreEqual(0.5f, Eval(expr));

            expr = Multiply(Constant(1), Constant(2f));
            Assert.AreEqual(2f, Eval(expr));

            expr = Multiply(Constant(1f), Constant(2f));
            Assert.AreEqual(2f, Eval(expr));
        }

        [TestMethod]
        public void Modulo_Int32()
        {
            ExpressionContext ctx = new ExpressionContext();

            var expr = Modulo(Constant(10), Constant(2));
            Assert.AreEqual(0, Eval(expr));

            expr = Modulo(Constant(9), Constant(2));
            Assert.AreEqual(1, Eval(expr));
        }

        [TestMethod]
        public void Negate_Int32()
        {
            var expr = Negate(Constant(0));
            Assert.AreEqual(0, Eval(expr));

            expr = Negate(Constant(1));
            Assert.AreEqual(-1, Eval(expr));

            expr = Negate(Constant(-1));
            Assert.AreEqual(1, Eval(expr));
        }
        [TestMethod]
        public void Negate_Variable()
        {
            var expr = Negate(Variable<int>("i"));
            Assert.AreEqual(0, Eval(new Dictionary<string, object>() { { "i", 0 } }, expr));

            expr = Negate(Variable<int>("i"));
            Assert.AreEqual(-1, Eval(new Dictionary<string, object>() { { "i", 1 } }, expr));

            expr = Negate(Variable<int>("i"));
            Assert.AreEqual(1, Eval(new Dictionary<string, object>() { { "i", -1 } }, expr));
        }
        [TestMethod]
        public void Assign_Int32()
        {
            var expr = Assign(Variable<int>("i"), Constant(2));
            Assert.AreEqual(2, Eval(new Dictionary<string, object>() { { "i", 1 } }, expr));
        }
        [TestMethod]
        public void AddAssign_Int32()
        {
            var expr = AddAssign(Variable<int>("i"), Constant(2));
            Assert.AreEqual(3, Eval(new Dictionary<string, object>() { { "i", 1 } }, expr));
        }
        [TestMethod]
        public void SubtractAssign_Int32()
        {
            var expr = SubtractAssign(Variable<int>("i"), Constant(2));
            Assert.AreEqual(-1, Eval(new Dictionary<string, object>() { { "i", 1 } }, expr));
        }

        [TestMethod]
        public void MultiplyAssign_Int32()
        {
            var expr = MultiplyAssign(Variable<int>("i"), Constant(3));
            Assert.AreEqual(6, Eval(new Dictionary<string, object>() { { "i", 2 } }, expr));
        }

        [TestMethod]
        public void DivideAssign_Float32()
        {
            var expr = DivideAssign(Variable<int>("i"), Constant(2f));
            Assert.AreEqual(0.5f, Eval(new Dictionary<string, object>() { { "i", 1 } }, expr));
        }

        [TestMethod]
        public void ModuloAssign_Int32()
        {
            var expr = ModuloAssign(Variable<int>("i"), Constant(2));
            Assert.AreEqual(0, Eval(new Dictionary<string, object>() { { "i", 10 } }, expr));

            expr = Modulo(Variable<int>("i"), Constant(2));
            Assert.AreEqual(1, Eval(new Dictionary<string, object>() { { "i", 9 } }, expr));
        }

        [TestMethod]
        public void MethodCall_()
        {
            var ctx = new ExpressionContext();

            var obj = new MethodClass();
            var objType = typeof(MethodClass);
            var varTarget = Variable(objType, "target");
            var result = Eval(new Dictionary<string, object>() { { "target", obj } },
                Call(varTarget, "GetMethod"), ctx);
            Assert.AreEqual(10, result);

            result = Eval(new Dictionary<string, object>() { { "target", obj } },
                Call(varTarget, "SetMethod", Constant(1)), ctx);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void MethodCall_Overload()
        {
            var ctx = new ExpressionContext();

            var obj = new MethodClass();
            var objType = obj.GetType();
            ctx.AddVariable<MethodClass>("target", obj);
            var varTarget = Variable(objType, "target");
            var result = Eval(new Dictionary<string, object>() { { "target", obj } },
                Call(varTarget, "OverloadMethod", Constant(1)), ctx);
            Assert.AreEqual(1, result);

            result = Eval(new Dictionary<string, object>() { { "target", obj } },
                Call(varTarget, "OverloadMethod", Constant("hello world")), ctx);

            Assert.AreEqual("hello world", result);

        }



        class MethodClass
        {
            private int value = 10;

            public int GetMethod()
            {
                return value;
            }
            public int SetMethod(int n)
            {
                value = n;
                return value;
            }

            #region 重载方法

            public int OverloadMethod(int n)
            {
                return n;
            }
            public string OverloadMethod(string str)
            {
                return str;
            }

            #endregion

        }




        //[TestMethod]
        //public void Return_()
        //{
        //    var ctx = new ExpressionContext();
        //    using (ctx.BeginScope())
        //    {
        //        var eval = ctx.Compile(Return(Constant(1)));
        //        eval();
        //        Assert.AreEqual(1, ctx.GotoValue);
        //    }
        //}

        [TestMethod]
        public void IfThen_()
        {
            Assert.AreEqual(1, Eval(IfThen(Constant(true), Constant(1))));
            Assert.AreEqual(null, Eval(IfThen(Constant(false), Constant(1))));
        }
        [TestMethod]
        public void IfThenElse_()
        {
            Assert.AreEqual(1, Eval(IfThenElse(Constant(true), Constant(1), Constant(2))));
            Assert.AreEqual(2, Eval(IfThenElse(Constant(false), Constant(1), Constant(2))));
        }

        [TestMethod]
        public void Switch_()
        {
            var ctx = new ExpressionContext();
            var varInt1 = Variable<int>("int1");
            Eval(new Dictionary<string, object>() { { "int1", 0 } },
                Switch(Constant(1), new SwitchCase(Constant(1), Assign(varInt1, Constant(1))),
               new SwitchCase(Constant(2), Assign(varInt1, Constant(2)))),
                ctx);

            Assert.AreEqual(1, ctx["int1"]);

            Eval(new Dictionary<string, object>() { { "int1", 0 } },
                Switch(Constant(2), new SwitchCase(Constant(1), Assign(varInt1, Constant(1))),
                    new SwitchCase(Constant(2), Assign(varInt1, Constant(2)))),
                ctx);

            Assert.AreEqual(2, ctx["int1"]);
        }

        class Class1
        {

        }

        public class MyValue
        {
            public MyValue()
            {

            }
            public MyValue(string value)
            {
                this.Value = value;
            }
            public string Value { get; set; }
            public static implicit operator MyValue(string p)
            {
                return new MyValue(p);
            }

            public static implicit operator string(MyValue p)
            {
                return p.Value;
            }


            [OverrideOperator]
            public static string Convert(MyValue v)
            {
                return v.Value;
            }

            [OverrideOperator(ExpressionType.Convert)]
            public static MyValue Convert(string str)
            {
                return new MyValue(str);
            }


            [OverrideOperator(ExpressionType.Convert)]
            public static MyValue3 Convert2(string str)
            {
                return new MyValue3(str);
            }

        }

        public class MyValue2 : MyValue
        {
            public MyValue2(string str)
                : base(str) { }
        }

        public class MyValue3 : MyValue
        {
            public MyValue3(string str)
                : base(str)
            {
            }
            public static implicit operator MyValue3(string p)
            {
                return new MyValue3(p);
            }
        }



        [TestMethod]
        public void Type_Convert()
        {
            MyValue my = new MyValue("a");
            string myStr = my;
            Assert.AreEqual("a", myStr);
            my = myStr;
            Assert.AreEqual("a", my.Value);

            MyValue2 my2 = new MyValue2("a2");
            myStr = my2;
            //my2 = myStr;//can't convert
            Assert.AreEqual("a2", myStr);
            MyValue3 my3 = myStr;
            myStr = my3;



            Func<object, Type, object> convert = (value, toType) =>
              {
                  Type fromType = value.GetType();
                  InvocationDelegate eval;
                  var compile = new CompileContext();

                  compile.OverrideOperator(typeof(MyValue));
                  compile.AddVariable(fromType, "a");
                  using (compile.BeginScope())
                  {
                      var expr = Convert(Variable(fromType, "a"), toType);

                      eval = compile.Compile(expr);
                  }

                  ExpressionContext ctx = new ExpressionContext();
                  ctx.AddVariable(fromType, "a");
                  ctx.SetVariable("a", value);
                  return eval(ctx);
              };

            object result;
            result = convert(new MyValue("Hello World"), typeof(string));
            Assert.AreEqual("Hello World", result);

            result = convert("Hello World", typeof(MyValue));
            Assert.AreEqual("Hello World", ((MyValue)result).Value);

            result = convert(new MyValue2("Hello World"), typeof(string));
            Assert.AreEqual("Hello World", result);

            try
            {
                result = convert("Hello World", typeof(MyValue2));
                Assert.AreEqual("Hello World", ((MyValue2)result).Value);
                Assert.Fail();
            }
            catch (OperatorNotImplementedException ex) { }

            result = convert(new MyValue3("Hello World"), typeof(string));
            Assert.AreEqual("Hello World", result);

            result = convert("Hello World", typeof(MyValue3));
            Assert.AreEqual("Hello World", ((MyValue3)result).Value);

        }
        [TestMethod]
        public void Type_Convert_int32_float32()
        {
            var expr= Convert(Constant(1), typeof(float));
            Assert.AreEqual(1f, Eval(expr));
        }

        [TestMethod]
        public void Type_As()
        {

            var expr = TypeAs(Variable<ArrayList>("a"), typeof(IList));
            Assert.AreEqual(typeof(ArrayList), Eval(new Dictionary<string, object>() { { "a", new ArrayList() } }, expr).GetType());
            expr = TypeAs(Variable<ArrayList>("a"), typeof(IDictionary));
            Assert.IsNull(Eval(new Dictionary<string, object>() { { "a", new ArrayList() } }, expr));

            try
            {
                expr = TypeAs(Variable<ArrayList>("a"), typeof(int));
                Assert.Fail();
            }
            catch (ArgumentException ex) { }
        }

        [TestMethod]
        public void Type_Is()
        {
            var expr = TypeIs(Constant(1), typeof(int));
            Assert.IsTrue(Eval<bool>(expr));
            expr = TypeIs(Constant(1), typeof(float));
            Assert.IsFalse(Eval<bool>(expr));
        }

        [TestMethod]
        public void PreIncrement_()
        {
            var expr = PreIncrement(Variable<int>("a"));
            Assert.AreEqual(1, Eval(new Dictionary<string, object>() { { "a", 1 } }, expr));
            expr = PreIncrement(Variable<float>("a"));
            Assert.AreEqual(0.1f, Eval(new Dictionary<string, object>() { { "a", 0.1f } }, expr));
        }

        [TestMethod]
        public void PreDecrement_()
        {
            var expr = PreDecrement(Variable<int>("a"));
            Assert.AreEqual(1, Eval(new Dictionary<string, object>() { { "a", 1 } }, expr));
            expr = PreDecrement(Variable<float>("a"));
            Assert.AreEqual(0.1f, Eval(new Dictionary<string, object>() { { "a", 0.1f } }, expr));
        }
        [TestMethod]
        public void PostIncrement_()
        {
            var expr = PostIncrement(Variable<int>("a"));
            Assert.AreEqual(2, Eval(new Dictionary<string, object>() { { "a", 1 } }, expr));
            expr = PostIncrement(Variable<float>("a"));
            Assert.AreEqual(1.1f, Eval(new Dictionary<string, object>() { { "a", 0.1f } }, expr));
        }

        [TestMethod]
        public void PostDecrement_()
        {
            var expr = PostDecrement(Variable<int>("a"));
            Assert.AreEqual(0, Eval(new Dictionary<string, object>() { { "a", 1 } }, expr));
            expr = PostDecrement(Variable<float>("a"));
            Assert.AreEqual(-0.9f, Eval(new Dictionary<string, object>() { { "a", 0.1f } }, expr));
        }
    }

}


