using LWJ.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static LWJ.Expressions.Expression;
using System.Collections.Generic;

namespace Test.LWJ.Expressions
{

    [TestClass]
    public class MemberTest : TestBase
    {
        [TestMethod]
        public void Property_GetValue()
        {
            var obj = new MemberClass();
            obj.IntProperty = 10;
            var getPropertyExpr = Property(Variable<MemberClass>("target"), "IntProperty");

            var result = Eval(new Dictionary<string, object>() { { "target", obj } }, getPropertyExpr);
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void Property_SetValue()
        {
            var obj = new MemberClass();
            var getPropertyExpr = Assign(Property(Variable<MemberClass>("target"), "IntProperty"), Constant(1));

            Eval(new Dictionary<string, object>() { { "target", obj } }, getPropertyExpr);
            Assert.AreEqual(1, obj.IntProperty);
        }

        [TestMethod]
        public void Field_GetValue()
        {
            object result;
            var obj = new MemberClass();
            var expr = PropertyOrField(Variable<MemberClass>("target"), "IntField");

            result = Eval(new Dictionary<string, object>() { { "target", obj } }, expr);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Field_SetValue()
        {
            var obj = new MemberClass();
            var setPropertyExpr = Assign(PropertyOrField(Variable<MemberClass>("target"), "IntField"), Constant(1));

            Eval(new Dictionary<string, object>() { { "target", obj } }, setPropertyExpr);
            Assert.AreEqual(1, obj.IntField);
        }

        class MemberClass
        {
            public int IntProperty { get; set; }

            public int IntField;
        }


    }
}
