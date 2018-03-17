using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml;
using static LWJ.Expressions.Expression;


namespace LWJ.Expressions.Xml.Test
{
    [TestClass]
    public class WriteTest
    {
        XmlExpressionWriter writer = new XmlExpressionWriter();

        [TestMethod]
        public void RootNoNamespce()
        {
            XmlDocument doc = new XmlDocument();
            var container = doc.CreateElement("expr");
            doc.AppendChild(container);
            var expr = Not(False);
            writer.Write(expr, container);
            string xml = container.OuterXml;
            Console.WriteLine(xml);
            Assert.AreEqual("<expr><not xmlns=\"" + XmlExpressionWriter.ExpressionNamespace + "\"><false /></not></expr>",
                xml);
        }

        [TestMethod]
        public void DefaultNamespce()
        {
            XmlDocument doc = new XmlDocument();
            var container = doc.CreateElement("", "expr", XmlExpressionReader.ExpressionNamespace);

            doc.AppendChild(container);
            var expr = Not(False);
            writer.Write(expr, container);
            string xml = container.OuterXml;
            Console.WriteLine(xml);
            Assert.AreEqual("<expr xmlns=\"" + XmlExpressionWriter.ExpressionNamespace + "\"><not><false /></not></expr>",
                xml);
        }

        [TestMethod]
        public void CustomPrefixNamespce()
        {
            XmlDocument doc = new XmlDocument();
            var container = doc.CreateElement("my", "expr", XmlExpressionReader.ExpressionNamespace);

            doc.AppendChild(container);
            var expr = Not(False);
            writer.Write(expr, container);
            string xml = container.OuterXml;
            Console.WriteLine(xml);
            Assert.AreEqual("<my:expr xmlns:my=\"" + XmlExpressionWriter.ExpressionNamespace + "\"><my:not><my:false /></my:not></my:expr>",
                xml);
        }


        [TestMethod]
        public void Constant()
        {
            Constant<int>(1);
        }

        string ToString(Expression expr)
        {
            return null;
        }

    }
}
