using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml;

namespace LWJ.Expressions.Xml.Test
{
    [TestClass]
    public class CalculateTest
    {

        [TestMethod]
        public void Sum()
        {
            string xml = Resource1.sum;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            var root = doc.DocumentElement;

            var compile = new CompileContext();
            var reader = new XmlExpressionReader();
            var expr = reader.Read(root.FirstChild);

            var eval = compile.Compile(expr);

            var result = (Delegate)eval(null);

            int n = 5;
            Assert.AreEqual(RecursiveSum(n), result.DynamicInvoke(n));
        }

        static int RecursiveSum(int n)
        {
            return n <= 1 ? 1 : n + RecursiveSum(--n);
        }
    }
}

