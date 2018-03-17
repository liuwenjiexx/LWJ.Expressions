using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LWJ.Expressions;
using System.Collections.ObjectModel;
using static LWJ.Expressions.Expression;
using System.Threading;

namespace Test.LWJ.Expressions
{
    [TestClass]
    public class UnitTest1
    {


        [TestMethod]
        public void FuncType()
        {
            Type funcType = GetFuncType(typeof(int), typeof(string));
            Assert.AreEqual(typeof(Func<int, string>), funcType);
        }

        //[TestMethod]
        //public void FuncVoidType()
        //{
        //    Type funcType = GetFuncType(typeof(int), typeof(void));
        //    Assert.AreEqual(typeof(Func<int, void>), funcType);
        //}
        //[ExpectedException(typeof(Exception))]
        //[TestMethod]
        //public void FuncTypeTest()
        //{
        //    var type = GetFuncType(typeof(int));
        //    var del = Delegate.CreateDelegate(type, GetType().GetMethod("Funt"));
        //}

        //public object Funt()
        //{
        //    return 1;
        //}

        [TestMethod]
        public void ActionType()
        {
            Type actionType = GetActionType(typeof(int), typeof(string));
            Assert.AreEqual(typeof(Action<int, string>), actionType);
        }

        [TestMethod]
        public void TestAssign()
        {
            var aa = new MyClass();
            var b = (aa.MyProperty = 1);
            Assert.AreEqual(1, b);
        }
        class MyClass
        {
            int n;
            public int MyProperty
            {
                get => -1;
                set => n = value;
            }
        }


        [TestMethod]
        public void TestDefaultLang()
        {

            var culture = Thread.CurrentThread.CurrentCulture;
            Console.WriteLine("default culture:" + culture.DisplayName + ", " + culture.Name);
            culture = Thread.CurrentThread.CurrentUICulture;
            Console.WriteLine("default ui culture:" + culture.DisplayName + ", " + culture.Name);
            Console.WriteLine("hello:" + Resource1.Hello);

            culture = new System.Globalization.CultureInfo("zh-CN");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            Console.WriteLine("culture:" + culture.DisplayName + ", " + culture.Name);
            Console.WriteLine("hello:" + Resource1.Hello);

            culture = new System.Globalization.CultureInfo("zh-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            Console.WriteLine("culture:" + culture.DisplayName + ", " + culture.Name);
            Console.WriteLine("hello:" + Resource1.Hello);


            culture = new System.Globalization.CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            Console.WriteLine("culture:" + culture.DisplayName + ", " + culture.Name);
            Console.WriteLine("hello:" + Resource1.Hello);
        }

        [TestMethod]
        public void TestReadArray()
        {
            //System.Linq.Expressions.Expression
            int[] arr = new int[] { 1, 2, 3 };
            ReadOnlyCollection<int> read = new ReadOnlyCollection<int>(arr);
            arr[0] = 2;
            Assert.AreEqual(2, read[0]);
        }

        [TestMethod]
        public void TestReadArray2()
        {
            int[] arr = new int[] { 1, 2, 3 };
            TestReadArray2(arr);
            Assert.AreEqual(-1, arr[0]);
        }

        public void TestReadArray2(params int[] arr)
        {
            arr[0] = -1;
            Console.WriteLine(arr[0]);
        }
        [TestMethod]
        public void TestMethod1()
        {

            Type type = typeof(Class2);
            Console.WriteLine(type.FullName);
            type = type.BaseType;
            Console.WriteLine(type.FullName);
            type = type.BaseType;
            Console.WriteLine(type.FullName);
        }

        private class Class1
        {

        }

        class Class2 : Class1
        {

        }



        private int runCount = 100000;
        [TestMethod]
        public void Time_Add()
        {

            var compile = new CompileContext();
            //using (compile.BeginScope())
            //{
            var expr = Add(Constant(1), Constant(2));
            var eval = compile.Compile(expr);
            DebugTime("Add(int)", () => eval(null), runCount);
            //}

        }

        [TestMethod]
        public void Time_Subtract()
        {

            var compile = new CompileContext();

            var expr = Subtract(Constant(1), Constant(2));
            var eval = compile.Compile(expr);
            DebugTime("Subtract(int)", () => eval(null), runCount);

        }


        [TestMethod]
        public void TestTime()
        {
            AddDelegate add = AddOperator1;
            Delegate del;
            del = add;
            Add2Delegate addObj = AddOperator2;

            Delegate delObj = addObj;

            int count = 100000;

            DebugTime("Delegate(int)", () =>
             {
                 add(1, 2);
             }, count);


            DebugTime("Delegate(Object)", () =>
            {
                addObj(1, 2);
            }, count);

            var method = add.Method;
            object target = this;
            DebugTime(" Method Invoke(int)", () =>
            {
                method.Invoke(target, new object[] { 1, 2 });
            }, count);
            var methodObj = addObj.Method;
            DebugTime(" Method Invoke(object)", () =>
            {
                methodObj.Invoke(target, new object[] { 1, 2 });
            }, count);

            DebugTime("DynamicInvoke(int)", () =>
             {
                 del.DynamicInvoke(1, 2);
             }, count);

            DebugTime("Delegate Method Invoke(int)", () =>
            {
                del.Method.Invoke(del.Target, new object[] { 1, 2 });
            }, count);
            DebugTime("DynamicInvoke(object)", () =>
            {
                delObj.DynamicInvoke(1, 2);
            }, count);

            DebugTime("Delegate Method Invoke(object)", () =>
            {
                delObj.Method.Invoke(delObj.Target, new object[] { 1, 2 });
            }, count);

            Func<int> empty1 = () =>
             {
                 int n = 1 + 2;
                 return n;
             };
            Delegate empty = empty1;
            DebugTime("empty1", () =>
            {
                empty1();
            }, count);
            DebugTime("empty1 dynamic ", () =>
            {
                empty.DynamicInvoke();
            }, count);
        }

        public static TimeSpan DebugTime(Action action, int count = 1)
        {
            var start = DateTime.Now;

            for (int i = 0; i < count; i++)
                action();

            return DateTime.Now - start;
        }
        public static void DebugTime(string text, Action action, int count = 1)
        {
            Console.WriteLine(text + (int)DebugTime(action, count).TotalMilliseconds);
        }


        delegate int AddDelegate(int a, int b);

        delegate int Add2Delegate(object a, object b);

        static int AddOperator1(int oper1, int oper2) => oper1 + oper2;

        static int AddOperator2(object oper1, object oper2) => (int)oper1 + (int)oper2;

    }
}
