using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml;

namespace LWJ.Expressions.Xml.Test
{
    [TestClass]
    public class XmlExpresionTest : TestBase
    {



        //[TestMethod]
        //public void Constant_()
        //{
        //    string xml = Resource1.constant;
        //    Run(xml);
        //}

        /*
         n=5
         5+4+3+2+1
             
             */
        int Sum(int n)
        {
            if (n <= 1)
                return 1;
            var b = Sum(n - 1);
            var result = n * b;
            Console.WriteLine("{0}={1}*{2}", result, n, b);
            return result;
        }


        [TestMethod]
        public void Run()
        {
            Console.WriteLine(Sum(5));
        }


        [TestMethod]
        public void NamedType_()
        {

            string xml = Resource1.named_type;
            Run(xml);
        }
        [TestMethod]
        public void If_()
        {
            string xml = Resource1._if;
            Run(xml);
        }
        [TestMethod]
        public void Or_()
        {
            string xml = Resource1.or;
            Run(xml);
        }
        [TestMethod]
        public void Variable_()
        {
            string xml = Resource1.variable;
            Run(xml);
        }

        [TestMethod]
        public void Loop_()
        {
            string xml = Resource1.loop;
            Run(xml);
        }

        [TestMethod]
        public void Switch_()
        {
            string xml = Resource1._switch;
            LoadXml(xml, (name, eval) =>
            {
                switch (name)
                {
                    case "switch":
                        {
                            var func = (Func<object, object>)eval(null);
                            Assert.AreEqual(10, func(1));
                            Assert.AreEqual(20, func(2));
                            Assert.AreEqual(20, func(3));
                            Assert.AreEqual(-1, func(4));
                        }
                        break;
                }
            });
        }

        [TestMethod]
        public void Func_()
        {
            string xml = Resource1.function;
            Run(xml);
        }

        [TestMethod]
        public void Switch1_()
        {
            //string sss = typeof(Func<>).FullName;
            //var ss = Type.GetType("System.Func`1");
            //var aa = GetFuncType(typeof(string));
            //aa = GetFuncType(typeof(string), typeof(string), typeof(string));
        }



    }
}
