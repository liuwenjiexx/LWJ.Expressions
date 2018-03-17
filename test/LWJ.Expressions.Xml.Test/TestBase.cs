using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LWJ.Expressions.Xml.Test
{ /*ExecutionEngineException: Attempting to JIT compile method '...' 
while running with --aot-only.*/
    public class TestBase
    {
        static Dictionary<string, string> cachedTypeNames;


        public TestBase()
        {

            if (cachedTypeNames == null)
            {
                cachedTypeNames = new Dictionary<string, string>();

                cachedTypeNames["int"] = typeof(int).FullName;
                cachedTypeNames["int32"] = typeof(int).FullName;
                cachedTypeNames["int64"] = typeof(long).FullName;
                cachedTypeNames["long"] = typeof(long).FullName;
                cachedTypeNames["float"] = typeof(float).FullName;
                cachedTypeNames["float32"] = typeof(float).FullName;
                cachedTypeNames["float64"] = typeof(double).FullName;
                cachedTypeNames["double"] = typeof(double).FullName;
                cachedTypeNames["bool"] = typeof(bool).FullName;
                cachedTypeNames["boolean"] = typeof(bool).FullName;
                cachedTypeNames["string"] = typeof(string).FullName;
                cachedTypeNames["type"] = typeof(Type).FullName;
                cachedTypeNames["datetime"] = typeof(DateTime).FullName;
            }
        }

        protected void Run(string xml, CompileContext compile = null, ExpressionContext ctx = null)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            var root = doc.DocumentElement;


            if (ctx == null)
                ctx = new ExpressionContext();
            if (compile == null)
            {
                compile = new CompileContext();
            }
            var reader = new XmlExpressionReader();

            foreach (XmlNode exprNode in root)
            {
                if (exprNode.NodeType != XmlNodeType.Element)
                    continue;
                string testName = exprNode.Attributes["name"].Value;
                Console.WriteLine("run:" + testName);

                var expr = reader.Read(exprNode, true, compile);

                var eval = compile.Compile(expr);

                object result = eval(ctx);
                object assertResult = null;
                var resultAttr = exprNode.Attributes["result"];
                if (resultAttr != null)
                {
                    Type assertResultType = exprNode.Attributes["resultType"] != null ? Type.GetType(GetTypeName(exprNode.Attributes["resultType"].Value), true) : typeof(string);
                    string text = exprNode.Attributes["result"].Value;
                    if (assertResultType == typeof(Type))
                        assertResult = Type.GetType(text);
                    else
                        assertResult = Convert.ChangeType(text, assertResultType);
                }

                Assert.AreEqual(assertResult, result, string.Format("name: {0}", testName));
            }

        }

        static string GetTypeName(string typeName)
        {
            if (typeName != null)
            {
                string name;
                if (cachedTypeNames.TryGetValue(typeName, out name))
                    return name;
            }
            return typeName;
        }



        public static void LoadXmlNode(string xml, Action<string, XmlNode> callback)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            foreach (XmlNode node in doc.SelectNodes("*/expr"))
            {
                var attr = node.Attributes["name"];
                string name = attr == null ? "" : attr.Value;
                callback(name, node);
            }
        }

        public static void LoadXml(string xml, Action<string, InvocationDelegate> callback)
        {
            LoadXmlNode(xml, new Action<string, XmlNode>((name, node) =>
            {
                XmlExpressionReader reader = new XmlExpressionReader();
                XmlNode firstChild = null;
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.NodeType == XmlNodeType.Element)
                    {
                        firstChild = child;
                        break;
                    }
                }
                //convert xml node to expression
                Expression expr = reader.Read(firstChild);

                CompileContext compile = new CompileContext();

                //compile expression
                var eval = compile.Compile(expr);

                callback(name, eval);
            }));
        }

    }
}
