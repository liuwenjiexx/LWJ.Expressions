/**************************************************************
 *  Filename:    XmlExpressionWriter.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;


namespace LWJ.Expressions.Xml
{
    public class XmlExpressionWriter
    {

        private Dictionary<ExpressionType, Action<XmlExpressionWriter, Expression>> writers;

        private static Dictionary<Type, string> cachedTypeNames;
        private static Dictionary<ExpressionType, Action<XmlExpressionWriter, Expression>> cachedWriters;
        private static object lockObj = new object();
        private object lockThisObj = new object();

        public const string ExpressionNamespace = XmlExpressionReader.ExpressionNamespace;

        private Stack<XmlNode> nodes;

        public XmlExpressionWriter()
        {
            lock (lockObj)
            {
                InitDefaultMember();
            }
        }

        public XmlNode CurrentNode { get => nodes.Peek(); }

        public void Write(Expression expr, XmlNode parent)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            lock (lockThisObj)
            {
                if (nodes == null)
                    nodes = new Stack<XmlNode>();
                nodes.Push(parent);
                WriteExpression(expr);
                nodes.Pop();
            }
        }


        public void WriteStartExpression(string nodeName, string ns)
        {
            XmlNode parent = CurrentNode;
            string nodePrefix = GetNodePrefix(parent, ns);
            var doc = parent.OwnerDocument;

            XmlNode exprNode;
            exprNode = doc.CreateElement(nodePrefix, nodeName, ns);
            parent.AppendChild(exprNode);

            nodes.Push(exprNode);
        }

        public void WriteEndExpression()
        {
            nodes.Pop();
        }

        static string GetNodePrefix(XmlNode node, string @namespace)
        {
            string nodePrefix;
            if (node.NamespaceURI == @namespace)
            {
                nodePrefix = node.Prefix;
            }
            else
            {
                nodePrefix = node.GetPrefixOfNamespace(@namespace);
                //if (string.IsNullOrEmpty(nodePrefix))
                //{
                //    nodePrefix = "x";
                //}
            }
            return nodePrefix;
        }

        public void AddWriter(ExpressionType exprType, Action<XmlExpressionWriter, Expression> writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            if (writers == null)
                writers = new Dictionary<ExpressionType, Action<XmlExpressionWriter, Expression>>(ExpressionType.None.EqualityComparer());
            writers[exprType] = writer;
        }



        public void WriteExpression(Expression expr)
        {
            if (expr.ExpressionType == ExpressionType.None)
                throw new ExpressionWriteException(Resource1.Write_ExprTypeNone, expr);
            Action<XmlExpressionWriter, Expression> writer;
            var exprType = expr.ExpressionType;
            if (writers == null || !writers.TryGetValue(exprType, out writer))
            {
                if (!cachedWriters.TryGetValue(exprType, out writer))
                {
                    throw new ExpressionWriteException(Resource1.Write_NotWriter, expr);
                }
            }
            writer(this, expr);
        }

        public void WriteExpressions(IEnumerable<Expression> exprs)
        {
            if (exprs == null)
                return;
            foreach (var expr in exprs)
            {
                WriteExpression(expr);
            }
        }

        private void WriteAttribute(XmlNode node, string attributeName, string value)
        {
            var attr = node.OwnerDocument.CreateAttribute(attributeName);
            attr.Value = value;
        }

        public void WriteAttribute(string attributeName, string value)
        {
            XmlNode node = CurrentNode;
            var attr = node.OwnerDocument.CreateAttribute(attributeName, node.NamespaceURI);
            attr.Value = value;
        }

        private string ToExprTypeName(Type type)
        {
            string typeName;
            if (!cachedTypeNames.TryGetValue(type, out typeName))
            {
                typeName = type.FullName;
            }
            return typeName;
        }

        private string ToExprTypeName(IEnumerable<Type> types)
        {
            if (types == null)
                return null;
            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (var type in types)
            {
                if (first)
                {
                    sb.Append(",");
                    first = false;
                }
                sb.Append(ToExprTypeName(type));
            }
            return sb.ToString();
        }

        private static void WriteTypeExpression(XmlExpressionWriter writer, Type type)
        {
            writer.WriteStartExpression("type", ExpressionNamespace);

            if (type != null)
                writer.WriteInnerText(writer.ToExprTypeName(type));
            writer.WriteEndExpression();
        }

        private static void WriteConstant(XmlExpressionWriter writer, Expression expr)
        {
            var constExpr = (ConstantExpression)expr;
            string typeName;
            Type valueType = constExpr.ValueType;
            if (valueType == typeof(bool))
            {
                if ((bool)constExpr.Value)
                    writer.WriteStartExpression("true", ExpressionNamespace);
                else
                    writer.WriteStartExpression("false", ExpressionNamespace);
                writer.WriteEndExpression();
                return;
            }
            else if (expr == Expression.Null)
            {
                writer.WriteStartExpression("null", ExpressionNamespace);
                writer.WriteEndExpression();
                return;
            }

            if (!cachedTypeNames.TryGetValue(valueType, out typeName))
                throw new TypeWriteException(Resource1.Write_UnknowConstantType, expr, valueType);

            writer.WriteStartExpression(typeName, ExpressionNamespace);

            string innerText = null;
            if (valueType == typeof(Type))
            {
                if (constExpr.Value != null)
                    innerText = ((Type)constExpr.Value).FullName;
            }
            else
            {
                if (constExpr.Value != null)
                    innerText = constExpr.Value.ToString();
            }
            writer.CurrentNode.InnerText = innerText;
            writer.WriteEndExpression();
        }


        private static void WriteBinaryExpr(XmlExpressionWriter writer, Expression expr)
        {
            BinaryExpression binary = (BinaryExpression)expr;

            string nodeName ;
            
            switch (expr.ExpressionType)
            {
                case ExpressionType.Equal:
                    nodeName = "eq";
                    break;
                case ExpressionType.NotEqual:
                    nodeName = "neq";
                    break;
                case ExpressionType.LessThan:
                    nodeName = "lt";
                    break;
                case ExpressionType.LessThanOrEqual:
                    nodeName = "leq";
                    break;
                case ExpressionType.GreaterThan:
                    nodeName = "gt";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    nodeName = "geq";
                    break;
                default:
                    nodeName= expr.ExpressionType.ToString();
                    nodeName = nodeName.Substring(0, 1).ToLower().ToString() + nodeName.Substring(1);
                    break;
            }

            writer.WriteStartExpression(nodeName, ExpressionNamespace);

            if (binary.Left != null)
                writer.WriteExpression(binary.Left);
            if (binary.Right != null)
                writer.WriteExpression(binary.Right);

            writer.WriteEndExpression();
        }

        private static void WriteUnaryExpr(XmlExpressionWriter writer, Expression expr)
        {
            UnaryExpression unary = (UnaryExpression)expr;

            var nodeName = expr.ExpressionType.ToString();
            nodeName = nodeName.Substring(0, 1).ToLower().ToString() + nodeName.Substring(1);
            writer.WriteStartExpression(nodeName, ExpressionNamespace);
            if (unary.Operand != null)
                writer.WriteExpression(unary.Operand);
            writer.WriteEndExpression();

        }

        private void WriteDefineVariable(IEnumerable<ParameterExpression> variables, bool allowIgnore = true)
        {
            if (variables == null) return;

            XmlNode varsNode = null;

            if (!allowIgnore)
            {
                WriteStartExpression("vars", ExpressionNamespace);
                varsNode = CurrentNode;
            }

            foreach (var variable in variables)
            {
                if (varsNode == null)
                {
                    WriteStartExpression("vars", ExpressionNamespace);
                    varsNode = CurrentNode;
                }
                WriteDefineVariable(this, variable);
            }
            if (varsNode != null)
                WriteEndExpression();
        }

        private static void WriteDefineVariable(XmlExpressionWriter writer, ParameterExpression variable)
        {
            writer.WriteStartExpression("var", ExpressionNamespace);
            writer.WriteInnerText(variable.Name);
            writer.WriteAttribute("type", writer.ToExprTypeName(variable.ValueType));
            writer.WriteEndExpression();
        }

        public void WriteInnerText(string text)
        {
            CurrentNode.InnerText = text;
        }

        private static void WriteVariable(XmlExpressionWriter writer, Expression expr)
        {
            ParameterExpression variable = (ParameterExpression)expr;
            writer.WriteStartExpression("var", ExpressionNamespace);
            writer.WriteInnerText(variable.Name);
            writer.WriteEndExpression();
        }

        private static void WriteFunction(XmlExpressionWriter writer, Expression expr)
        {
            FunctionExpression func = (FunctionExpression)expr;
            writer.WriteStartExpression("func", ExpressionNamespace);
            writer.WriteDefineVariable(func.Arguments);
            writer.WriteExpression(func.Body);
            writer.WriteAttribute("name", func.Name);
            writer.WriteAttribute("returnType", writer.ToExprTypeName(func.ReturnType));
            writer.WriteEndExpression();
        }

        private static void WriteCall(XmlExpressionWriter writer, Expression expr)
        {
            MethodCallExpression call = (MethodCallExpression)expr;
            writer.WriteStartExpression("call", ExpressionNamespace);
            var method = call.Method;
            if (method.IsStatic)
            {
                writer.WriteExpression(Expression.Null);
            }
            else
            {
                writer.WriteExpression(call.Instance);
            }
            writer.WriteExpressions(call.Arguments);
            writer.WriteAttribute("method", method.Name);
            if (call.Instance.ValueType != method.DeclaringType)
                writer.WriteAttribute("type", writer.ToExprTypeName(method.DeclaringType));
            writer.WriteAttribute("argTypes", writer.ToExprTypeName(method.GetParameters().Select(o => o.ParameterType).ToArray()));

            writer.WriteEndExpression();
        }

        private static void WriteBlock(XmlExpressionWriter writer, Expression expr)
        {
            BlockExpression block = (BlockExpression)expr;
            writer.WriteStartExpression("block", ExpressionNamespace);
            writer.WriteDefineVariable(block.Variables);
            writer.WriteExpressions(block.Expressions);
            writer.WriteEndExpression();
        }

        private static void WriteMember(XmlExpressionWriter writer, Expression expr)
        {
            ＭemberExpression member = (ＭemberExpression)expr;

            if (member.IsPropertyOrField)
            {
                writer.WriteStartExpression("member", ExpressionNamespace);
            }
            else
            {
                if (member.IsProperty)
                    writer.WriteStartExpression("property", ExpressionNamespace);
                else
                    writer.WriteStartExpression("field", ExpressionNamespace);
            }

            writer.WriteAttribute("name", member.Member.Name);

            if (member.ValueType != member.Member.DeclaringType)
            {
                writer.WriteAttribute("type", writer.ToExprTypeName(member.Member.DeclaringType));
            }

            if (member.Instance != null)
                writer.WriteExpression(member.Instance);
            else
                writer.WriteExpression(Expression.Null);
            writer.WriteEndExpression();
        }

        private static void WriteGoto(XmlExpressionWriter writer, Expression expr)
        {
            GotoExpression gotoExpr = (GotoExpression)expr;
            switch (gotoExpr.GotoType)
            {
                case GotoType.Return:
                    WriteUnaryExpr(writer, expr);
                    break;
                case GotoType.Break:
                    writer.WriteStartExpression("break", ExpressionNamespace);
                    writer.WriteEndExpression();
                    break;
                case GotoType.Continue:
                    writer.WriteStartExpression("continue", ExpressionNamespace);
                    writer.WriteEndExpression();
                    break;
            }

        }

        private static void WriteConditional(XmlExpressionWriter writer, Expression expr)
        {
            ConditionalExpression cond = (ConditionalExpression)expr;
            writer.WriteStartExpression("if", ExpressionNamespace);
            writer.WriteExpression(cond.Test);
            writer.WriteExpression(cond.IfTrue);

            Expression falseExpr = cond.IfFalse;

            if (falseExpr != null)
            {
                var elseIfExpr = falseExpr as ConditionalExpression;
                while (elseIfExpr != null)
                {
                    writer.WriteStartExpression("elseIf", ExpressionNamespace);
                    writer.WriteExpression(elseIfExpr);
                    writer.WriteEndExpression();
                    falseExpr = elseIfExpr.IfFalse;
                    elseIfExpr = falseExpr as ConditionalExpression;
                }

                if (falseExpr != null)
                {
                    writer.WriteStartExpression("else", ExpressionNamespace);
                    writer.WriteExpression(cond.IfFalse);
                    writer.WriteEndExpression();
                }
            }
            writer.WriteEndExpression();
        }


        private static void WriteLoop(XmlExpressionWriter writer, Expression expr)
        {
            LoopExpression loop = (LoopExpression)expr;
            writer.WriteStartExpression("loop", ExpressionNamespace);
            writer.WriteExpression(loop.Body);
            writer.WriteEndExpression();
        }

        private static void WriteSwitch(XmlExpressionWriter writer, Expression expr)
        {
            SwitchExpression switchExpr = (SwitchExpression)expr;
            writer.WriteStartExpression("switch", ExpressionNamespace);
            writer.WriteExpression(switchExpr.TestValue);
            if (switchExpr.Cases != null)
            {
                foreach (var switchCase in switchExpr.Cases)
                {
                    writer.WriteStartExpression("case", ExpressionNamespace);
                    writer.WriteExpressions(switchCase.TestValues);
                    if (switchCase.Body != null)
                    {
                        writer.WriteExpression(switchCase.Body);
                    }
                    writer.WriteEndExpression();
                }
            }
            if (switchExpr.DefaultValue != null)
            {
                writer.WriteStartExpression("default", ExpressionNamespace);
                writer.WriteExpression(switchExpr.DefaultValue);
                writer.WriteEndExpression();
            }
            writer.WriteEndExpression();
        }

        private void InitDefaultMember()
        {
            if (cachedWriters != null)
                return;
            cachedWriters = new Dictionary<ExpressionType, Action<XmlExpressionWriter, Expression>>(ExpressionType.None.EqualityComparer());

            cachedWriters[ExpressionType.Constant] = WriteConstant;
            cachedWriters[ExpressionType.Variable] = WriteVariable;
            cachedWriters[ExpressionType.Block] = WriteBlock;
            cachedWriters[ExpressionType.Function] = WriteFunction;
            cachedWriters[ExpressionType.Call] = WriteCall;
            cachedWriters[ExpressionType.Member] = WriteMember;
            cachedWriters[ExpressionType.Goto] = WriteGoto;
            cachedWriters[ExpressionType.Conditional] = WriteConditional;
            cachedWriters[ExpressionType.Loop] = WriteLoop;
            cachedWriters[ExpressionType.Switch] = WriteSwitch;


            foreach (var item in new ExpressionType[] {
                ExpressionType.Add,
                ExpressionType.Subtract,
                ExpressionType.Multiply,
                ExpressionType.Divide,
                ExpressionType.Modulo,
                ExpressionType.Assign,
                ExpressionType.AddAssign,
                ExpressionType.SubtractAssign,
                ExpressionType.MultiplyAssign,
                ExpressionType.ModuloAssign,
                ExpressionType.Equal,ExpressionType.NotEqual,
                ExpressionType.LessThan, ExpressionType.LessThanOrEqual,
                ExpressionType.GreaterThan, ExpressionType.GreaterThanOrEqual,
             ExpressionType.And, ExpressionType.Or})
            {
                cachedWriters[item] = WriteBinaryExpr;
            }

            foreach (var item in new ExpressionType[] {
                ExpressionType.Negate,
                ExpressionType.PreIncrement,
                ExpressionType.PostIncrement,
                ExpressionType.PreDecrement,
                ExpressionType.PostDecrement,
            ExpressionType.Not
             })
            {
                cachedWriters[item] = WriteUnaryExpr;
            }

            cachedTypeNames = new Dictionary<Type, string>();


            cachedTypeNames[typeof(int)] = "int";
            cachedTypeNames[typeof(long)] = "long";
            cachedTypeNames[typeof(float)] = "float";
            cachedTypeNames[typeof(double)] = "double";
            cachedTypeNames[typeof(bool)] = "bool";
            cachedTypeNames[typeof(string)] = "string";
            cachedTypeNames[typeof(DateTime)] = "datetime";
            cachedTypeNames[typeof(Type)] = "type";


        }

        //write convert typeAs typeIs

    }


}
