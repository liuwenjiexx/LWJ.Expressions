/**************************************************************
 *  Filename:    XmlExpressionReader.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using static LWJ.Expressions.Expression;

namespace LWJ.Expressions.Xml
{
    public class XmlExpressionReader
    {

        private Dictionary<string, Dictionary<string, Func<XmlExpressionReader, Expression>>> readers;
        private XmlNamespaceManager nsmgr;

        private static Dictionary<string, Func<XmlExpressionReader, Expression>> cachedReader;
        private static Dictionary<string, string> cachedTypeNames;

        public const string ExpressionNamespace = "urn:schema-lwj:expression";
        private static readonly object lockObj = new object();
        private readonly object lockThisObj = new object();
        private Stack<XmlNode> nodes;
        private Stack<CompileContext> compileContexts;

        public XmlExpressionReader()
        {
            lock (lockObj)
            {
                InitDefaultMember();
            }
        }

        /// <summary>
        /// current node
        /// </summary>
        public XmlNode Node
        {
            get
            {
                if (nodes.Count == 0)
                    throw new MemberAccessException("Not Invoke " + nameof(ReadStartExpression));
                var node = nodes.Peek();
                return node;
            }
        }

        /// <summary>
        /// current context scope
        /// </summary>
        public CompileContext Context
        {
            get
            {
                if (compileContexts.Count == 0)
                    throw new MemberAccessException("Not Invoke " + nameof(PushScope));
                var ctx = compileContexts.Peek();
                return ctx;
            }
        }

        /// <summary>
        /// xml text to an expression
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="isBlock"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public Expression Read(string xml, bool isBlock = false, CompileContext ctx = null)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            var exprNode = doc.DocumentElement;
            if (exprNode == null)
                return null;
            return Read(exprNode, isBlock, ctx);
        }

        /// <summary>
        /// xml node to expression
        /// </summary>
        /// <param name="exprNode">expression node or expression container node</param>
        /// <param name="isBlock">if is true, parse <paramref name="exprNode"/> child nodes, if is false, parse <paramref name="exprNode"/></param>
        /// <param name="ctx">provider <see cref="CompileContext"/></param>
        /// <returns></returns>
        public Expression Read(XmlNode exprNode, bool isBlock = false, CompileContext ctx = null)
        {
            if (exprNode == null) throw new ArgumentNullException(nameof(exprNode));
            Expression expr;

            lock (lockThisObj)
            {
                if (ctx == null)
                {
                    ctx = CompileContext.Current;
                    if (ctx == null)
                        ctx = new CompileContext();

                }
                nsmgr = new XmlNamespaceManager(exprNode.OwnerDocument.NameTable);
                nsmgr.AddNamespace("x", ExpressionNamespace);
                nodes = new Stack<XmlNode>();
                compileContexts = new Stack<CompileContext>();

                using (ctx.BeginScope())
                {
                    compileContexts.Push(ctx);
                    PushScope();
                    ReadStartExpression(exprNode);
                    if (isBlock)
                        expr = ReadBlock(this);
                    else
                        expr = ReadExpression();
                    ReadEndExpression();
                    PopScope();
                }
                nsmgr = null;
                nodes = null;
                compileContexts = null;
            }

            return expr;
        }

        /// <summary>
        /// read any expression before calling this method
        /// </summary>
        /// <param name="node"></param>
        public void ReadStartExpression(XmlNode node)
        {
            nodes.Push(node);
        }

        /// <summary>
        /// must <see cref="ReadStartExpression(XmlNode)"/> pairs
        /// </summary>
        public void ReadEndExpression()
        {
            nodes.Pop();
        }

        /// <summary>
        /// reads current node as an expression
        /// </summary>
        /// <returns></returns>
        public Expression ReadExpression()
        {
            Func<XmlExpressionReader, Expression> reader = null;
            var node = Node;
            if (readers != null)
            {
                Dictionary<string, Func<XmlExpressionReader, Expression>> reader2;
                if (readers.TryGetValue(node.NamespaceURI, out reader2))
                {
                    reader2.TryGetValue(node.LocalName, out reader);
                }
            }
            if (reader == null && node.NamespaceURI == ExpressionNamespace)
            {
                cachedReader.TryGetValue(node.LocalName, out reader);
            }
            if (reader == null)
                throw new UnknowNodeNameException(node);

            var expr = reader(this);

            return expr;
        }

        /// <summary>
        /// add custom expression reader
        /// </summary>
        /// <param name="nodeName">node local name</param>
        /// <param name="ns">node namespace</param>
        /// <param name="reader"></param>
        public void AddReader(string nodeName, string ns, Func<XmlExpressionReader, Expression> reader)
        {
            if (nodeName == null) throw new ArgumentNullException(nameof(nodeName));
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            if (readers == null)
                readers = new Dictionary<string, Dictionary<string, Func<XmlExpressionReader, Expression>>>();

            Dictionary<string, Func<XmlExpressionReader, Expression>> reader2;
            if (!readers.TryGetValue(ns, out reader2))
            {
                reader2 = new Dictionary<string, Func<XmlExpressionReader, Expression>>();
                readers[ns] = reader2;
            }
            reader2[nodeName] = reader;
        }

        /// <summary>
        /// create new <see cref="Context"/> scope
        /// </summary>
        public void PushScope()
        {
            var ctx = new CompileContext(Context);
            compileContexts.Push(ctx);
        }

        /// <summary>
        /// must <see cref="PushScope"/> pairs
        /// </summary>
        public void PopScope()
        {
            compileContexts.Pop();
        }

        private static Expression ReadConstant(XmlExpressionReader reader)
        {
            Type type = reader.ReadAttributeValue<Type>("type");
            object value;
            var node = reader.Node;
            value = reader.ChangeType(node, reader.ReadInnerText(), type);
            return Constant(value, type);
        }


        private static Expression ReadType(XmlExpressionReader reader)
        {
            string typeName = reader.ReadInnerText();

            Type type = FindType(typeName);
            if (type == null)
                throw new InvalidConvertReadException(reader.Node, typeName, typeof(Type));
            return Constant(type, type);
        }

        static Type FindType(string typeName, bool throwOnError = false, bool ignoreCase = false)
        {
            if (typeName != null)
                typeName = GetTypeName(typeName);
            return Type.GetType(typeName, throwOnError, ignoreCase);
        }


        private static Func<XmlExpressionReader, Expression> ToReadTypedValue(Type valueType)
        {
            return (reader) =>
            {
                object value;
                var node = reader.Node;
                value = reader.ChangeType(node, reader.ReadInnerText(), valueType);

                return Constant(value, valueType);
            };
        }

        public object ChangeType(XmlNode node, string text, Type valueType)
        {
            object value;
            try
            {
                if (valueType == typeof(Type))
                {
                    value = FindType(text, true);
                }
                else
                {
                    value = System.Convert.ChangeType(text, valueType);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidConvertReadException(node, text, valueType, ex);
            }
            return value;
        }


        private static ParameterExpression ReadDefineVariable(XmlExpressionReader reader)
        {
            Type type = reader.ReadAttributeValue<Type>("type");
            string name = reader.ReadInnerText();
            reader.Context.AddVariable(type, name);

            return Variable(type, name);
        }
        private static Expression ReadVariable(XmlExpressionReader reader)
        {
            string name = reader.ReadInnerText();
            Type type = reader.GetVariableType(name);
            return Variable(type, name);
        }


        private static ParameterExpression[] ReadDefineVariables(XmlExpressionReader reader)
        {
            return FilterChildNodeTypes(reader.Node).Select(o =>
            {
                reader.ReadStartExpression(o);
                var expr = ReadDefineVariable(reader);
                reader.ReadEndExpression();
                return expr;
            }).ToArray();
        }

        private static Expression ReadMethodCall(XmlExpressionReader reader)
        {
            string methodName = reader.ReadAttributeValue("method", null);
            Expression callExpr;
            reader.PushScope();

            var exprs = reader.ReadChildExpressions();

            if (exprs == null || exprs.Length == 0)
                throw new MissingNodeException(reader.Node, null);

            if (string.IsNullOrEmpty(methodName))
            {
                Type returnType = exprs[0].ValueType;
                //function
                callExpr = Call(exprs[0], returnType, exprs.Skip(1));
            }
            else
            {
                Type type = reader.ReadAttributeValue<Type>("type", null);
                Type[] argTypes = ReadTypes(reader.ReadAttributeValue("argTypes", null));
                Expression instance = exprs[0];
                //if (instance == Null)
                //    instance = null;
                //if (instance != null)
                //{
                callExpr = Call(instance, type ?? instance.ValueType, methodName, argTypes, exprs.Skip(1));
                //}
                //else
                //{
                //    callExpr = Call(type, methodName, argTypes, exprs.Skip(1));
                //}
            }

            reader.PopScope();
            return callExpr;
        }

        public Expression[] ReadChildExpressions(out ParameterExpression[] variables)
        {
            var items = FilterChildNodeTypes(Node);
            var first = items.FirstOrDefault();
            IEnumerable<Expression> exprs;
            if (first.LocalName == "vars")
            {
                ReadStartExpression(first);
                variables = ReadDefineVariables(this);
                ReadEndExpression();
                items = items.Skip(1);
            }
            else
            {
                variables = null;
            }
            exprs = items.Select(o =>
            {
                ReadStartExpression(o);
                var expr = ReadExpression();
                ReadEndExpression();
                return expr;
            });
            return exprs.ToArray();
        }

        public Expression[] ReadChildExpressions(int index = 0, int count = -1)
        {
            var items = FilterChildNodeTypes(Node).Skip(index);
            if (count != -1)
                items = items.Take(count);
            return items.Select(o =>
            {
                ReadStartExpression(o);
                var expr = ReadExpression();
                ReadEndExpression();
                return expr;
            }).ToArray();
        }

        private static Type[] ReadTypes(string typesString)
        {
            if (string.IsNullOrEmpty(typesString))
                return null;
            string[] parts = typesString.Split(',');
            Type[] types = new Type[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                types[i] = FindType(parts[i], true);
            }
            return types;
        }

        private static Expression ReadReturn(XmlExpressionReader reader)
        {
            var exprs = reader.ReadChildExpressions();

            if (exprs == null && exprs.Length == 0)
                return Return();

            return Return(exprs[0]);
        }

        private static Expression ReadProperty(XmlExpressionReader reader)
        {
            string propertyName = reader.ReadAttributeValue("name");
            Type type = reader.ReadAttributeValue<Type>("type", null);
            Expression instance;
            var exprs = reader.ReadChildExpressions();
            if (exprs == null || exprs.Length == 0)
                throw new MissingNodeException(Resource1.Read_NotInstanceOrTypeNode, reader.Node, null);
            instance = exprs[0];
            //if (exprs[0] is ConstantExpression)
            //    type = ((ConstantExpression)exprs[0]).Value as Type;
            //if (type == null)
            //{
            //    return Property(exprs[0], propertyName);
            //}
            return Property(instance, type ?? instance.ValueType, propertyName);
        }

        private static Expression ReadField(XmlExpressionReader reader)
        {
            string fieldName = reader.ReadAttributeValue("name");
            Type type = reader.ReadAttributeValue<Type>("type", null);
            Expression instance;
            var exprs = reader.ReadChildExpressions();
            if (exprs == null || exprs.Length == 0)
                throw new MissingNodeException(Resource1.Read_NotInstanceOrTypeNode, reader.Node, null);
            instance = exprs[0];
            //if (exprs[0] is ConstantExpression)
            //    type = ((ConstantExpression)exprs[0]).Value as Type;

            //if (type == null)
            //{
            //    return Field(exprs[0], fieldName);
            //}
            return Field(instance, type ?? instance.ValueType, fieldName);
        }

        private static Expression ReadMember(XmlExpressionReader reader)
        {
            string propertyOrFieldName = reader.ReadAttributeValue("name");
            Type type = reader.ReadAttributeValue<Type>("type", null);
            Expression instance;
            var exprs = reader.ReadChildExpressions();
            if (exprs == null || exprs.Length == 0)
                throw new MissingNodeException(Resource1.Read_NotInstanceOrTypeNode, reader.Node, null);
            instance = exprs[0];
            //if (exprs[0] is ConstantExpression)
            //    type = ((ConstantExpression)exprs[0]).Value as Type;

            //if (type == null)
            //{
            //    return PropertyOrField(instance, propertyOrFieldName);
            //}
            return PropertyOrField(instance, type ?? instance.ValueType, propertyOrFieldName);
        }

        private static Expression ReadBlock(XmlExpressionReader reader)
        {
            ParameterExpression[] vars = null;
            reader.PushScope();
            var exprs = reader.ReadChildExpressions(out vars);
            reader.PopScope();
            return Block(vars, exprs);
        }

        private static Expression ReadIf(XmlExpressionReader reader)
        {
            Expression ifFalse = null;

            var elseNode = reader.Node.SelectSingleNode("x:else", reader.nsmgr);
            if (elseNode != null)
            {
                var child = FilterChildNodeTypes(elseNode).FirstOrDefault();
                if (child != null)
                {
                    reader.ReadStartExpression(child);
                    ifFalse = reader.ReadExpression();
                    reader.ReadEndExpression();
                }
            }

            return ReadIf(reader, ifFalse);
        }

        private static ConditionalExpression ReadIf(XmlExpressionReader reader, Expression ifFalse)
        {
            var childs = reader.ReadChildExpressions(0, 2);
            Expression test, ifTrue;
            if (childs == null || childs.Length == 0)
                throw new MissingNodeException(Resource1.Read_NotTestValueNode, reader.Node, null);
            if (childs.Length == 1)
                throw new ExpressionReadException(Resource1.Read_NotBodyNode, reader.Node);
            test = childs[0];
            ifTrue = childs[1];

            var elseIfNodes = reader.Node.SelectNodes("x:elseIf", reader.nsmgr);
            for (int i = elseIfNodes.Count - 1; i >= 0; i--)
            {
                var child = elseIfNodes[i];
                reader.ReadStartExpression(child);
                ifFalse = ReadIf(reader, ifFalse);
                reader.ReadEndExpression();
            }

            if (childs.Length == 1)
                return IfThenElse(test, null, ifFalse);

            return IfThenElse(test, ifTrue, ifFalse);
        }


        private static Expression ReadSwitch(XmlExpressionReader reader)
        {
            var childs = reader.FilterChildNodeTypes();
            var first = childs.FirstOrDefault();
            if (first == null)
                throw new MissingNodeException(Resource1.Read_NotTestValueNode, reader.Node, null);
            Expression testValue;

            Expression defaultBody = null;

            reader.ReadStartExpression(first);
            testValue = reader.ReadExpression();
            reader.ReadEndExpression();

            List<SwitchCase> cases = new List<SwitchCase>();

            foreach (XmlNode child in childs.Skip(1))
            {
                if (child.LocalName == "case")
                {
                    reader.ReadStartExpression(child);
                    var exprs = reader.ReadChildExpressions();

                    if (exprs == null || exprs.Length == 0)
                        throw new MissingNodeException(Resource1.Read_NotTestValueNode, child, null);
                    if (exprs.Length < 2)
                        throw new NotBodyNodeException(child);

                    var testValues = exprs.Take(exprs.Length - 1);
                    var body = exprs.Last();
                    SwitchCase sc = new SwitchCase(testValues, body);
                    cases.Add(sc);

                    reader.ReadEndExpression();
                }
                else if (child.LocalName == "default")
                {
                    reader.ReadStartExpression(child);
                    var exprs = reader.ReadChildExpressions();
                    if (exprs == null || exprs.Length == 0)
                        throw new NotBodyNodeException(child);
                    if (defaultBody != null)
                        throw new DuplicateNodeException(child);
                    defaultBody = exprs.First();
                    reader.ReadEndExpression();
                }
            }

            return Switch(testValue, defaultBody, cases);
        }

        private static Expression ReadLoop(XmlExpressionReader reader)
        {
            var exprs = reader.ReadChildExpressions(0, 2);

            if (exprs.Length == 0)
                throw new NotBodyNodeException(reader.Node);

            Expression test, body, loop;

            test = exprs[0];
            if (exprs.Length == 2)
            {
                body = exprs[1];
                loop = Loop(Block(IfThen(Not(test), Break()), body));
            }
            else
            {
                loop = Loop(Block(IfThen(Not(test), Break())));
            }
            return loop;
        }

        private static Expression ReadFunction(XmlExpressionReader reader)
        {
            ParameterExpression[] variables = null;
            Expression body = null;

            string name = reader.ReadAttributeValue("name", null);
            Type returnType = reader.ReadAttributeValue<Type>("returnType", typeof(void));
            if (!string.IsNullOrEmpty(name))
                reader.Context.AddVariable(returnType, name);

            var exprs = reader.ReadChildExpressions(out variables);
            if (exprs == null || exprs.Length == 0)
                throw new NotBodyNodeException(reader.Node);
            if (exprs != null && exprs.Length > 0)
                body = exprs[0];

            return Function(name, variables, body, returnType);
        }

        private static TypeUnaryExpression ReadTypeConvert(XmlExpressionReader reader)
        {
            var childs = reader.ReadChildExpressions(0, 2);
            Type convertToType = childs[1].ValueType;

            return Convert(childs[0], convertToType);
        }
        private static TypeBinaryExpresion ReadTypeAs(XmlExpressionReader reader)
        {
            var childs = reader.ReadChildExpressions(0, 2);
            Type convertToType = childs[1].ValueType;

            return TypeAs(childs[0], convertToType);
        }
        private static TypeBinaryExpresion ReadTypeIs(XmlExpressionReader reader)
        {
            var childs = reader.ReadChildExpressions(0, 2);
            Type convertToType = childs[1].ValueType;

            return TypeIs(childs[0], convertToType);
        }

        private IEnumerable<XmlNode> FilterChildNodeTypes()
        {
            return FilterChildNodeTypes(Node);
        }

        private static IEnumerable<XmlNode> FilterChildNodeTypes(XmlNode node)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType != XmlNodeType.Element)
                    continue;

                yield return child;
            }
        }

        private void InitDefaultMember()
        {
            if (cachedReader != null)
                return;
            cachedReader = new Dictionary<string, Func<XmlExpressionReader, Expression>>();


            cachedReader["type"] = ReadType;
            cachedReader["int"] = ToReadTypedValue(typeof(int));
            cachedReader["long"] = ToReadTypedValue(typeof(long));
            cachedReader["float"] = ToReadTypedValue(typeof(float));
            cachedReader["double"] = ToReadTypedValue(typeof(double));
            cachedReader["bool"] = ToReadTypedValue(typeof(bool));
            cachedReader["string"] = (reader) => Constant(reader.ReadInnerText(), typeof(string));
            cachedReader["datetime"] = ToReadTypedValue(typeof(DateTime));
            cachedReader["true"] = (reader) => True;
            cachedReader["false"] = (reader) => False;
            cachedReader["null"] = (reader) => Null;
            cachedReader["var"] = ReadVariable;
            cachedReader["call"] = ReadMethodCall;
            cachedReader["break"] = (reader) => Break();
            cachedReader["continue"] = (reader) => Continue();
            cachedReader["return"] = ReadReturn;
            cachedReader["member"] = ReadMember;
            cachedReader["property"] = ReadProperty;
            cachedReader["field"] = ReadField;
            cachedReader["block"] = ReadBlock;
            cachedReader["if"] = ReadIf;
            cachedReader["switch"] = ReadSwitch;
            cachedReader["loop"] = ReadLoop;
            cachedReader["func"] = ReadFunction;
            cachedReader["convert"] = ReadTypeConvert;
            cachedReader["as"] = ReadTypeAs;
            cachedReader["is"] = ReadTypeIs;

            new Func<Expression, Expression, BinaryExpression>[] {
                Add, Subtract, Multiply, Divide, Modulo, Equal, NotEqual, LessThan,
                LessThanOrEqual, GreaterThan, GreaterThanOrEqual ,Or,And}
            .Where(func =>
            {
                string name = func.Method.Name;
                name = (name[0] + "").ToLower() + name.Substring(1);

                switch (name)
                {
                    case "equal":
                        name = "eq";
                        break;
                    case "notEqual":
                        name = "neq";
                        break;
                    case "lessThan":
                        name = "lt";
                        break;
                    case "lessThanOrEqual":
                        name = "leq";
                        break;
                    case "greaterThan":
                        name = "gt";
                        break;
                    case "greaterThanOrEqual":
                        name = "geq";
                        break;
                }

                cachedReader[name] = new Func<XmlExpressionReader, Expression>((reader) =>
                {
                    Expression[] exprs = reader.ReadChildExpressions();
                    if (exprs == null || exprs.Length != 2)
                        throw new MissingNodeException(Resource1.Read_RequiresTwoChildNode, reader.Node, null);

                    return func(exprs[0], exprs[1]);
                });
                return true;
            }).ToArray();

            new Func<Expression, UnaryExpression>[] { Negate, Not }
            .Where(func =>
           {
               string name = func.Method.Name;
               name = (name[0] + "").ToLower() + name.Substring(1);

               cachedReader[name] = new Func<XmlExpressionReader, Expression>((reader) =>
              {
                  var exprs = reader.ReadChildExpressions();
                  if (exprs == null || exprs.Length != 1)
                      throw new MissingNodeException(Resource1.Read_RequiresAChildNode, reader.Node, null);
                  return func(exprs[0]);
              });
               return false;
           }).ToArray();

            new Func<AccessableExpression, Expression, BinaryExpression>[] {
                Assign,
                AddAssign, SubtractAssign, MultiplyAssign, DivideAssign, ModuloAssign }
           .Where(func =>
           {
               string name = func.Method.Name;
               name = (name[0] + "").ToLower() + name.Substring(1);

               cachedReader[name] = new Func<XmlExpressionReader, Expression>((reader) =>
              {
                  AccessableExpression left;
                  var exprs = reader.ReadChildExpressions();
                  if (exprs == null || exprs.Length != 2)
                      throw new MissingNodeException(Resource1.Read_RequiresTwoChildNode, reader.Node, null);

                  left = exprs[0] as AccessableExpression;
                  if (left == null)
                      throw new ExpressionReadException(Resource1.Read_NotAccessableNode, FilterChildNodeTypes(reader.Node).First());

                  return func(left, exprs[1]);
              });
               return false;
           }).ToArray();

            new Func<AccessableExpression, UnaryExpression>[] { PreIncrement, PostIncrement, PreDecrement, PostDecrement }
            .Where(func =>
            {
                string name = func.Method.Name;
                name = (name[0] + "").ToLower() + name.Substring(1);

                cachedReader[name] = new Func<XmlExpressionReader, Expression>((reader) =>
               {
                   AccessableExpression left;
                   var exprs = reader.ReadChildExpressions();
                   if (exprs == null || exprs.Length != 1)
                       throw new MissingNodeException(Resource1.Read_RequiresAChildNode, reader.Node, null);

                   left = exprs[0] as AccessableExpression;
                   if (left == null)
                       throw new ExpressionReadException(Resource1.Read_NotAccessableNode, FilterChildNodeTypes(reader.Node).First());
                   return func(left);
               });
                return false;
            }).ToArray();

            cachedTypeNames = new Dictionary<string, string>();


            cachedTypeNames["bool"] = typeof(bool).FullName;
            cachedTypeNames["string"] = typeof(string).FullName;
            cachedTypeNames["int"] = typeof(int).FullName;
            cachedTypeNames["long"] = typeof(long).FullName;
            cachedTypeNames["float"] = typeof(float).FullName;
            cachedTypeNames["double"] = typeof(double).FullName;
            cachedTypeNames["datetime"] = typeof(DateTime).FullName;
            cachedTypeNames["type"] = typeof(Type).FullName;


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


        public string ReadAttributeValue(string name, string defaultValue)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var attr = Node.Attributes.GetNamedItem(name);
            if (attr == null)
                return defaultValue;
            return attr.Value;
        }

        public T ReadAttributeValue<T>(string name, T defaultValue)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var attr = Node.Attributes.GetNamedItem(name);
            if (attr == null)
                return defaultValue;
            return (T)ChangeType(Node, attr.Value, typeof(T));
        }

        public T ReadAttributeValue<T>(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            var node = Node;
            var attr = node.Attributes.GetNamedItem(name);
            if (attr == null) throw new MissingAttributeException(node, name);

            return (T)ChangeType(node, attr.Value, typeof(T));
        }

        public string ReadAttributeValue(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            var node = Node;
            var attr = node.Attributes.GetNamedItem(name);
            if (attr == null) throw new MissingAttributeException(node, name);
            return attr.Value;
        }

        public string ReadInnerText()
        {
            return Node.InnerText;
        }


        public Type GetVariableType(string name)
        {
            var ctx = Context;
            return ctx.GetVariableType(name);
        }



    }


}
