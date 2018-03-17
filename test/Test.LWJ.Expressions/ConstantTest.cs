using LWJ.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static LWJ.Expressions.Expression;

namespace Test.LWJ.Expressions
{
    [TestClass]
    public class ConstantTest : TestBase
    {
        [TestMethod]
        public void Constant_Type()
        {
            object value;

            value = 1;

            ConstantExpression expr = Constant(value);
            Assert.AreEqual(value, expr.Value);
            Assert.AreEqual(expr.ValueType, typeof(int));
            Assert.AreEqual(value, Eval(expr));

            value = 1l;
            expr = Constant(value);
            Assert.AreEqual(value, expr.Value);
            Assert.AreEqual(expr.ValueType, typeof(long));
            Assert.AreEqual(value, Eval(expr));


            value = 1.0f;
            expr = Constant(value);
            Assert.AreEqual(value, expr.Value);
            Assert.AreEqual(expr.ValueType, typeof(float));
            Assert.AreEqual(value, Eval(expr));

            value = 1.0d;
            expr = Constant(value);
            Assert.AreEqual(value, expr.Value);
            Assert.AreEqual(expr.ValueType, typeof(double));
            Assert.AreEqual(value, Eval(expr));

            value = true;
            expr = Constant(value);
            Assert.AreEqual(value, expr.Value);
            Assert.AreEqual(expr.ValueType, typeof(bool));
            Assert.AreEqual(value, Eval(expr));

            value = true;
            expr = True;
            Assert.AreEqual(value, expr.Value);
            Assert.AreEqual(expr.ValueType, typeof(bool));
            Assert.AreEqual(value, Eval(expr));

            value = false;
            expr = False;
            Assert.AreEqual(value, expr.Value);
            Assert.AreEqual(expr.ValueType, typeof(bool));
            Assert.AreEqual(value, Eval(expr));
        }

        [TestMethod]
        public void Constant_String()
        {
            string value = "string";
            var expr = Constant(value);
            Assert.AreEqual(value, expr.Value);
            Assert.AreEqual(expr.ValueType, typeof(string));
            Assert.AreEqual(value, Eval(expr));

        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Constant_DbNull()
        {
            var value = DBNull.Value;
            var expr = Constant(value);
            Assert.AreEqual(value, expr.Value);
            Assert.AreEqual(expr.ValueType, typeof(DBNull));
            Assert.AreEqual(value, Eval(expr));
        }



        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Constant_NotIsPrimitive()
        {
            Constant(new object());
        }

    }
}
