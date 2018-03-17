using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml;

namespace LWJ.Expressions.Xml.Test
{
    [TestClass]
    public class CustomTest : TestBase
    {

        [TestMethod]
        public void Custom_Read()
        {
            string xml = Resource1.custom;

            var reader = new XmlExpressionReader();

            reader.AddReader( "hello", "urn:my", (r) =>
            {
                Assert.AreEqual(r, reader);
                var exprs = r.ReadChildExpressions();
                var operand = exprs[0];
                return new HelloExpression(operand);
            });

            LoadXmlNode(xml, (name, node) =>
            {
                var expr = reader.Read(node.FirstChild);
                CompileContext compile = new CompileContext();
                var eval = compile.Compile(expr);
                object result = eval(null);
                Assert.AreEqual("Hello World", result);
            });
        }

        [TestMethod]
        public void Custom_Write()
        {
            var expr = new HelloExpression(Expression.Constant<string>("Hello World"));
            var writer = new XmlExpressionWriter();
            writer.AddWriter((ExpressionType)100, (writer2, expr2) =>
            {
                HelloExpression helloExpr = (HelloExpression)expr2;
                writer2.WriteStartExpression("hello", "urn:my");
                writer2.WriteExpression(helloExpr.Operand);
                writer2.WriteEndExpression();
            });

            XmlDocument doc = new XmlDocument();
            var container = doc.CreateElement("expr");
            doc.AppendChild(container);
            writer.Write(expr, container);
            string xml = container.InnerXml;
            Console.WriteLine(xml);
            Assert.AreEqual("<hello xmlns=\"urn:my\"><string xmlns=\"" + XmlExpressionWriter.ExpressionNamespace + "\">Hello World</string></hello>",
                xml);

        }


        class HelloExpression : UnaryExpression
        {
            public HelloExpression(Expression operand)
                : base((ExpressionType)100, operand, null)
            {
            }

            public override Type ValueType => typeof(string);

            public override CompiledDelegate Compile(CompileContext ctx)
            {
                var value = Operand.Compile(ctx);
                return (invoke) => value(invoke);
            }


        }
 
    }
}
