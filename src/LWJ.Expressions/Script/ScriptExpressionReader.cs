using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LWJ.Expressions.Script
{

    public class ScriptExpressionReader
    {
        private static Dictionary<string, KeywordInfo> kws;
        private Dictionary<ExpressionType, KeywordInfo> kwTypes;
        static string operChar;
        //  private HashSet<char> kwSingle;

        private ScriptExpressionReader()
        {
            if (kws == null)
            {
                kws = new Dictionary<string, KeywordInfo>();
                operChar = "";
                // kwSingle = new HashSet<char>();
                Action<string, int> addKeyword = (keyword, priority) =>
                  {
                      kws[keyword] = new KeywordInfo()
                      {
                          Keyword = keyword,
                          Priority = priority,
                      };
                      //if (keyword.Length == 1)
                      //    kwSingle.Add(keyword[0]);
                      foreach (var ch in keyword)
                      {
                          if (operChar.IndexOf(ch) < 0)
                              operChar += ch;
                      }
                  };
                addKeyword(".", 0);
                addKeyword("(", 0);
                addKeyword("=", 0);
                addKeyword(",", 0);
                addKeyword("||", 2);
                addKeyword("&&", 2);
                addKeyword("==", 3);
                addKeyword("!=", 3);
                addKeyword("<", 3);
                addKeyword("<=", 3);
                addKeyword(">", 3);
                addKeyword(">=", 3);
                addKeyword("+", 4);
                addKeyword("-", 4);
                addKeyword("*", 5);
                addKeyword("/", 5);
                addKeyword("%", 5);
                addKeyword("!", 6);
                addKeyword(")", 7);
            }
            kwTypes = new Dictionary<ExpressionType, KeywordInfo>();


            //foreach (var cfgKw in ScriptConfig.Config.Keywords)
            //{
            //    KeywordInfo kw = new KeywordInfo();
            //    kw.Keyword = cfgKw.Keyword;
            //    kw.NodeType = cfgKw.Type;
            //    kw.Priority = cfgKw.Priority;
            //    kws.Add(kw.Keyword, kw);
            //    if (kw.NodeType != ExpressionType.None)
            //        kwTypes.Add(kw.NodeType, kw);
            //    if (kw.Keyword.Length == 1)
            //        kwSingle.Add(kw.Keyword[0]);
            //}

        }

        private static ScriptExpressionReader instance;
        public static ScriptExpressionReader Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScriptExpressionReader();
                }
                return instance;
            }
        }





        private class KeywordInfo
        {
            public string Keyword;
            public int Priority;
            public ExpressionType NodeType;
            public override string ToString()
            {
                return Keyword;
            }
        }
        public string ToKeyword(ExpressionType type)
        {

            KeywordInfo kw;
            if (Instance.kwTypes.TryGetValue(type, out kw))
                return kw.Keyword;

            return null;
        }

        //public bool IsSingleKeyword(char ch)
        //{
        //    return kwSingle.Contains(ch);
        //}

        private Expression GetExpr(PartInfo kw, Stack<Expression> s1, Stack<PartInfo> s2)
        {
            Expression left;
            Expression right;
            switch (kw.keywordInfo.Keyword)
            {
                case "+":
                    right = PopData(s1);
                    left = PopData(s1);
                    return Expression.Add(left, right);
                case "-":
                    right = PopData(s1);
                    if (kw.segment.Prev == null || (kw.segment.Prev.IsOper && kw.segment.Prev.Expr != ")" && kw.segment.Prev.Index + 1 == kw.segment.Index))
                    // if (s2.Count > 0 && s2.Peek().Keyword == "(")
                    {
                        return Expression.Negate(right);
                    }

                    left = PopData(s1);
                    return Expression.Subtract(left, right);
                case "*":
                    right = PopData(s1);
                    left = PopData(s1);
                    return Expression.Multiply(left, right);
                case "/":
                    right = PopData(s1);
                    left = PopData(s1);
                    return Expression.Divide(left, right);
                case "%":
                    right = PopData(s1);
                    left = PopData(s1);
                    return Expression.Modulo(left, right);
                case "<":
                    right = PopData(s1);
                    left = PopData(s1);
                    return Expression.LessThan(left, right);
                case "<=":
                    right = PopData(s1);
                    left = PopData(s1);
                    return Expression.LessThanOrEqual(left, right);
                case ">":
                    right = PopData(s1);
                    left = PopData(s1);
                    return Expression.GreaterThan(left, right);
                case ">=":
                    right = PopData(s1);
                    left = PopData(s1);
                    return Expression.GreaterThanOrEqual(left, right);
                case "==":
                    right = PopData(s1);
                    left = PopData(s1);
                    return Expression.Equal(left, right);
                case "!=":
                    right = PopData(s1);
                    left = PopData(s1);
                    return Expression.NotEqual(left, right);
                case "||":
                    right = PopData(s1);
                    left = PopData(s1);
                    return Expression.Or(left, right);
                case "&&":
                    right = PopData(s1);
                    left = PopData(s1);
                    return Expression.And(left, right);
                case "=":
                    right = PopData(s1);
                    left = PopData(s1);
                    return Expression.Assign((AccessableExpression)left, right);
                /*  case ",":
                      right = PopData(s1);
                      left = PopData(s1);

                      JoinExpression join = right as JoinExpression;

                      if (join == null)
                      {
                          join = new JoinExpression();
                          join.List.Add(left);
                          join.List.Add(right);
                      }
                      else
                      {
                          join.List.Insert(0, left);
                      }
                      return join;*/
                case ")":
                    while (s2.Count > 0 && s2.Peek().keywordInfo.Keyword != "(")
                    {
                        s1.Push(GetExpr(s2.Pop(), s1, s2));
                    }

                    if (s2.Count <= 0)
                        throw new Exception("not match '('");
                    s2.Pop();
                    left = s1.Pop();
                    return new GroupExpression(left);
            }
            throw new Exception("error keyword :" + kw);
        }

        Expression PopData(Stack<Expression> s1)
        {
            Expression exp = null;
            if (s1.Count > 0)
                exp = s1.Pop();

            //if (exp.ExpressionType == ExpressionType.Data)
            //{
            //    DataExpression d = (DataExpression)exp;
            //    while (d != null)
            //    {
            //        if (d.Expr.StartsWith("."))
            //        {
            //            Expression left = s1.Pop();
            //            d.Left = left;
            //            d = left as DataExpression;
            //        }
            //        else
            //        {
            //            break;
            //        }
            //    }
            //}

            return exp;
        }

        static bool IsDigit(char ch)
        {
            return ('0' <= ch && ch <= '9') || ch == '.';
        }
        static bool IsStartChar(char ch)
        {
            if (ch < 128)
            {
                return ('a' <= ch && ch <= 'z') || ('A' <= ch && ch <= 'Z') || (ch == '_') || (ch == '$');
            }
            return false;
        }
        static bool IsMemberChar(char ch)
        {

            if (ch < 128)
            {
                if (ch == '"')
                    return false;
                return IsStartChar(ch) || ('0' <= ch && ch <= '9');
            }
            return false;
        }
        static bool IsOper(char ch)
        {
            return operChar.IndexOf(ch) >= 0;
        }
        static bool isSpaceChar(char ch)
        {
            return ch == ' ' || ch == '\r' || ch == '\n';
        }

        IEnumerable<ExpressionSegment> ToParts(string expr)
        {
            if (string.IsNullOrEmpty(expr))
                yield break;

            int len = expr.Length;
            char ch;
            int offset = 0;
            int count = 0;
            bool isKw;
            string keyword = null;
            bool isWord = false;
            bool isDigit = false;
            int index = 0;
            bool isString = false;

            ExpressionSegment prev = null;
            ExpressionSegment current;
            for (int i = 0; i < len; i++)
            {
                ch = expr[i];
                isKw = false;

                if (offset == i)
                {
                    if (isSpaceChar(ch))
                    {
                        offset = i + 1;
                        count = 0;
                        continue;
                    }
                    isString = false;
                    isWord = false;
                    isDigit = false;
                    if (ch == '"')
                    {
                        isString = true;
                        count = 0;
                        continue;
                    }
                    else if (IsStartChar(ch))
                    {
                        isWord = true;
                    }
                    else
                    {
                        if (IsDigit(ch))
                        {
                            isDigit = true;
                        }
                    }
                    count = 1;
                    if (!isWord && !isDigit)
                    {
                        if (kws.ContainsKey(ch.ToString()))
                        {
                            bool isOper = true;
                            //if (ch == '(' || ch == ')')
                            //    isOper = false;
                            if (i < len - 1 && kws.ContainsKey(ch.ToString() + expr[i + 1]))
                            {
                                current = new ExpressionSegment()
                                {
                                    Index = index++,
                                    Expr = ch.ToString() + expr[i + 1],
                                    Offset = offset,
                                    Length = 2,
                                    IsKeyword = true,
                                    IsOper = isOper,
                                    Prev = prev
                                };
                                yield return current;
                                prev = current;
                                i++;
                                offset = i + 1;

                            }
                            else
                            {
                                current = new ExpressionSegment()
                                {
                                    Index = index++,
                                    Expr = ch.ToString(),
                                    Offset = offset,
                                    Length = 1,
                                    IsKeyword = true,
                                    IsOper = isOper,
                                    Prev = prev
                                };
                                yield return current;
                                prev = current;
                                offset = i + 1;
                            }
                        }
                    }
                    continue;

                }
                else
                {
                    if (isString)
                    {
                        if (ch != '"')
                        {
                            count++;
                            continue;
                        }
                        current = new ExpressionSegment()
                        {
                            Index = index++,
                            Expr = expr.Substring(offset + 1, count),
                            IsString = true,
                            Offset = offset,
                            Length = count + 2,
                            Prev = prev,
                        };
                        yield return current;
                        prev = current;
                        offset = i + 1;
                        isString = false;
                        continue;
                    }
                    else if (isDigit)
                    {
                        if (IsDigit(ch))
                        {
                            count++;
                            continue;
                        }
                    }
                    else if (isWord)
                    {
                        if (IsMemberChar(ch))
                        {
                            count++;
                            continue;
                        }

                    }
                    else
                    {

                        if (!IsStartChar(ch) && !IsDigit(ch) && ch != '"')
                        {
                            count++;
                            continue;
                        }

                    }

                }

                if (count > 0)
                {
                    current = new ExpressionSegment()
                    {
                        Index = index++,
                        Expr = expr.Substring(offset, count),
                        Offset = offset,
                        Length = count,
                        Prev = prev,
                    };
                    if (isDigit)
                        current.IsDigit = true;
                    else
                        current.IsOper = kws.ContainsKey(current.Expr);

                    yield return current;
                    prev = current;

                }
                i--;
                offset = i + 1;
                count = 0;
            }

            if (len > offset)
            {
                current = new ExpressionSegment()
                {
                    Index = index++,
                    Expr = expr.Substring(offset, len - offset),
                    Offset = offset,
                    Length = len - offset,
                    Prev = prev
                };
                if (isDigit)
                    current.IsDigit = true;
                else
                    current.IsOper = kws.ContainsKey(current.Expr);
                yield return current;
                prev = current;
            }
        }

        private Dictionary<string, Expression> cachedExprs = new Dictionary<string, Expression>();


        public Expression Parse(string expr, CompileContext ctx = null, Dictionary<Expression, ExpressionSegment> segments = null)
        {
            if (expr == null)
                throw new Exception("expr null");
            expr = expr.Trim();

            if (string.IsNullOrEmpty(expr))
                throw new Exception("expr empty");

            Expression result;

            if (cachedExprs.TryGetValue(expr, out result))
            {
                return result;
            }

            Stack<Expression> s1 = new Stack<Expression>();
            Stack<PartInfo> s2 = new Stack<PartInfo>();
            Stack<string> variables = new Stack<string>();
            Stack<CompileContext> ctxs = new Stack<CompileContext>();
            ctxs.Push(ctx);
            PushScope(ctxs);


            foreach (var segment in ToParts(expr))
            {
                if (segment.IsString)
                {
                    s1.Push(Expression.Constant<string>(segment.Expr));
                    //s1.Push((Expression)part1);
                    if (segments != null)
                        segments[s1.Peek()] = segment;
                    continue;
                }
                string exprPart = segment.Expr;
                Console.WriteLine("part:" + exprPart);
                if (kws.ContainsKey(exprPart))
                // if(segment.IsKeyword)
                {
                    var part = new PartInfo()
                    {
                        segment = segment,
                        keywordInfo = kws[exprPart]
                    };
                    var kw = part.keywordInfo;
                    if (kw.Keyword == ")")
                    {

                        while (s2.Count > 0 && s2.Peek().keywordInfo.Keyword != "(")
                        {
                            var tmp = s2.Pop();
                            s1.Push(GetExpr(tmp, s1, s2));

                            if (s2.Peek().keywordInfo.Keyword == ",")
                            {

                            }
                        }
                        Expression contentNode;
                        if (s1.Peek().ExpressionType != ExpressionType.Group)
                        {
                            contentNode = PopData(s1);
                        }
                        else
                        {
                            contentNode = null;
                        }
                        if (s1.Peek() is JoinExpression)
                        {
                            var join = s1.Pop() as JoinExpression;
                            //join.List.Insert(0, contentNode);
                            join.List.Add(contentNode);
                            contentNode = join;

                        }
                        //pop group
                        s1.Pop();
                        bool isGroup = true;

                        if (s1.Count > 0 && (s1.Peek().ExpressionType == ExpressionType.Member || s1.Peek().ExpressionType == ExpressionType.Variable))
                        {
                            Expression[] args = null;
                            if (contentNode == null)
                            {
                                args = null;
                            }
                            else if (contentNode is JoinExpression)
                            {
                                args = ((JoinExpression)contentNode).List.ToArray();
                            }
                            else
                            {
                                args = new Expression[] { contentNode };
                            }
                            var d = s1.Peek() as MemberExpression;
                            if (d != null)
                            {
                                if (d.Member.MemberType == MemberTypes.Method)
                                {
                                    s1.Pop();
                                    s1.Push(Expression.Call(d.Instance, d.Member.Name, args));
                                    if (segments != null)
                                    {
                                        var seg = segments[d];
                                        segments.Remove(d);
                                        segments[s1.Peek()] = seg;
                                    }
                                }
                            }
                            else
                            {
                                var p = s1.Peek() as ParameterExpression;

                                if (p.ValueType == typeof(MethodInfo))
                                {
                                    MethodInfo mInfo = ctx.Context.GetVariable(p.Name) as MethodInfo;
                                    s1.Pop();
                                    s1.Push(Expression.Call(mInfo, args));
                                    if (segments != null)
                                    {
                                        var seg = segments[p];
                                        segments.Remove(p);
                                        segments[s1.Peek()] = seg;
                                    }
                                }
                                else if (p.ValueType.IsSubclassOf(typeof(Delegate)))
                                {
                                    Type delType = p.ValueType;
                                    var mInfo = delType.GetMethod("Invoke");

                                    s1.Push(Expression.Call(s1.Pop(), mInfo.ReturnType, args));
                                    if (segments != null)
                                    {
                                        var seg = segments[p];
                                        segments.Remove(p);
                                        segments[s1.Peek()] = seg;
                                    }
                                }
                            }
                            isGroup = false;
                        }


                        if (isGroup)
                        {
                            GroupExpression g = new GroupExpression(contentNode);
                            s1.Push(g);
                        }


                        s2.Pop();
                    }
                    else if (kw.Keyword == ",")
                    {

                        while (s2.Count > 0 && s2.Peek().keywordInfo.Keyword != "(")
                        {
                            s1.Push(GetExpr(s2.Pop(), s1, s2));
                        }
                        var left = PopData(s1);
                        JoinExpression join;
                        if (s1.Count > 0 && s1.Peek() is JoinExpression)
                        {
                            join = s1.Peek() as JoinExpression;
                        }
                        else
                        {
                            join = new JoinExpression();
                            s1.Push(join);
                        }
                        //join.List.Insert(0, left);
                        join.List.Add(left);
                    }
                    else
                    {

                        if (kw.Keyword == "(")
                        {
                            s1.Push(Expression.Group(Expression.Null));

                        }

                        while (s2.Count > 0 && kw.Priority != 0 &&/* s2.Peek().Priority != 0 &&*/ s2.Peek().keywordInfo.Priority > kw.Priority)
                        {
                            var tmp = s2.Pop();
                            s1.Push(GetExpr(tmp, s1, s2));

                        }
                        s2.Push(part);
                    }
                }
                else
                {
                    switch (exprPart.ToLower())
                    {
                        case "true":
                            s1.Push(Expression.True);
                            if (segments != null)
                                segments[s1.Peek()] = segment;
                            continue;
                        case "false":
                            s1.Push(Expression.False);
                            if (segments != null)
                                segments[s1.Peek()] = segment;
                            continue;
                        case "null":
                            s1.Push(Expression.Null);
                            if (segments != null)
                                segments[s1.Peek()] = segment;
                            continue;
                            //case "undefined":
                            //    s1.Push(Expression.Undefined);
                            //    continue;
                    }

                    if (segment.IsDigit)
                    {
                        if (segment.Expr.IndexOf('.') >= 0)
                        {
                            double f;
                            if (double.TryParse(exprPart, out f))
                            {
                                s1.Push(Expression.Constant(f));
                                if (segments != null)
                                    segments[s1.Peek()] = segment;
                                continue;
                            }
                        }
                        else
                        {
                            long l;
                            if (long.TryParse(exprPart, out l))
                            {
                                s1.Push(Expression.Constant(l));
                                if (segments != null)
                                    segments[s1.Peek()] = segment;
                                continue;
                            }
                        }

                    }


                    if (s2.Count > 0 && s2.Peek().keywordInfo.Keyword == ".")
                    {

                        var variable = s1.Pop();
                        var member = GetMember(variable, null, exprPart, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.GetField | BindingFlags.GetProperty);
                        if (member == null)
                            throw new Exception("not found Member :" + exprPart);
                        if (member.MemberType == MemberTypes.Method)
                        {
                            s1.Push(Expression.Member(variable, null, exprPart));
                            if (segments != null)
                                segments[s1.Peek()] = segment;
                        }
                        else
                        {
                            s1.Push(Expression.PropertyOrField(variable, member.Name));
                            if (segments != null)
                                segments[s1.Peek()] = segment;
                        }

                        s2.Pop();
                    }

                    else
                    {
                        //variable
                        s1.Push(Expression.Variable(GetVariableType(ctxs, exprPart), exprPart));
                        if (segments != null)
                            segments[s1.Peek()] = segment;
                    }
                    //  variables.Push(part);

                }
            }


            while (s2.Count > 0)
            {
                var tmp = s2.Pop();
                s1.Push(GetExpr(tmp, s1, s2));
            }

            result = PopData(s1);



            if (s1.Count > 1)
                throw new Exception("expr stack error, count>1 count:" + s1.Count + ", expr:" + expr + " ,type:" + s1.Peek().ToString());

            PopScope(ctxs);

            //cachedExprs[expr] = result;

            return result;
        }

        public void PushScope(Stack<CompileContext> compileContexts)
        {
            var ctx = new CompileContext(compileContexts.Count > 0 ? compileContexts.Peek() : null);
            compileContexts.Push(ctx);
        }

        /// <summary>
        /// must <see cref="PushScope"/> pairs
        /// </summary>
        public void PopScope(Stack<CompileContext> compileContexts)
        {
            compileContexts.Pop();
        }


        public void AddVariable(Stack<CompileContext> compileContexts, string name, Type type)
        {
            compileContexts.Peek().AddVariable(type, name);
        }
        public Type GetVariableType(Stack<CompileContext> compileContexts, string name)
        {
            var ctx = compileContexts.Peek();
            return ctx.GetVariableType(name);
        }

        static MemberInfo GetMember(Expression instance, Type type, string memberName, BindingFlags flags)
        {
            Type objType = type;
            if (objType == null)
                objType = instance.ValueType;
            if (objType == null)
                throw new ArgumentNullException(nameof(type));

            var mInfo = objType.GetMember(memberName, flags);
            if (mInfo == null || mInfo.Length == 0)
                return null;
            return mInfo[0];
        }



        class JoinExpression : Expression
        {
            public List<Expression> List = new List<Expression>();
            public JoinExpression()
            {

            }
            public override CompiledDelegate Compile(CompileContext ctx)
            {
                throw new NotImplementedException();
            }
        }

        public class ExpressionSegment
        {
            public string Expr;
            public int Offset;
            public int Length;
            public bool IsString;
            public bool IsDigit;
            public bool IsKeyword;
            public bool IsOper;
            public int Index;
            public ExpressionSegment Prev;
        }

        class PartInfo
        {
            public ExpressionSegment segment;
            public KeywordInfo keywordInfo;
        }

    }

}
