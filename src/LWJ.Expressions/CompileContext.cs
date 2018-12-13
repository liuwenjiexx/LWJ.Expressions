using System;
using System.Collections.Generic;
using System.Reflection;

namespace LWJ.Expressions
{
    public class CompileContext
    {
        private CompileContext parent;


        public static Action<string> DebugLog;

        [System.Diagnostics.Conditional("LOG")]
        public static void Log(string message)
        {
            if (DebugLog != null)
                DebugLog(message);
        }


        private Dictionary<ExpressionType, Dictionary<Tuple<Type, Type>, OperatorInfo>> cachedBinaryOpers;

        private Dictionary<string, VariableInfo> variables;
        private Dictionary<string, object> variableMetadata;
        private BlockType blockType;
        private IExpressionContext context;

        private static CompileContext _default;
        private static CompileContext current;
        private static object lockObj = new object();
        public CompileContext(BlockType blockType = BlockType.Block)
            : this(null, null, blockType, false)
        {
        }

        public CompileContext(IExpressionContext context, BlockType blockType = BlockType.Block)
            : this(null, context, blockType, false)
        {
        }

        public CompileContext(CompileContext parent, BlockType blockType = BlockType.Block)
            : this(parent, null, blockType, false)
        {
        }
        public CompileContext(CompileContext parent, IExpressionContext context, BlockType blockType = BlockType.Block)
            : this(parent, context, blockType, false)
        {
        }

        private CompileContext(CompileContext parent, IExpressionContext context, BlockType blockType, bool isDefault)
        {
            variables = new Dictionary<string, VariableInfo>();
            variableMetadata = new Dictionary<string, object>();
            this.context = context;
            this.blockType = blockType;
            if (parent == null)
            {
                if (isDefault)
                    InitDefault();
                else
                    this.parent = Default;
            }
            else
            {
                this.parent = parent;
            }
        }

        internal static CompileContext Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new CompileContext(null, null, BlockType.Block, true);
                }
                return _default;
            }
        }
        public static CompileContext Current
        {
            get
            {
                lock (lockObj)
                {
                    return current;
                }
            }
        }

        public IExpressionContext Context => context;

        public BlockType BlockType => blockType;



        private void InitDefault(BlockType blockType = BlockType.Block)
        {

            OverrideOperator(typeof(DefaultOperators));
            if (cachedBinaryOpers.Count < 10)
                throw new Exception("Operators Count :" + cachedBinaryOpers.Count);
        }



        public void OverrideOperator(Type exportOperatorClass)
        {
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod;

            foreach (var method in exportOperatorClass.GetMethods(bindingFlags))
            {
                var operAttr = method.GetCustomAttribute<OverrideOperatorAttribute>(false);
                Log(" operator  method: " + method + ", operAttr null:" + (operAttr == null) + "  ,class: " + exportOperatorClass);
                if (operAttr == null)
                    continue;
                ExpressionType Operator = operAttr.OperatorType;
                if (Operator == ExpressionType.None && !string.IsNullOrEmpty(method.Name))
                {
                    if (Enum.IsDefined(typeof(ExpressionType), method.Name))
                        Operator = (ExpressionType)Enum.Parse(typeof(ExpressionType), method.Name);
                }

                if (Operator == ExpressionType.None)
                    throw new Exception("override operator ExpressionType.None, method name:" + method.Name + ", method:" + method);

                Log("operator: " + Operator + "  method name: " + method.Name + ", " + method);
                OverrideOperator(Operator, method);
            }
        }

        public void OverrideOperator(ExpressionType operatorType, MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            Type operType1 = null;
            Type operType2 = null;
            Type resultType = method.ReturnType;

            var arguments = method.GetParameters();
            if (arguments.Length == 1)
            {
                operType1 = arguments[0].ParameterType;
                if (operatorType == ExpressionType.Convert)
                    operType2 = method.ReturnType;

            }
            else if (arguments.Length == 2)
            {
                operType1 = arguments[0].ParameterType;
                operType2 = arguments[1].ParameterType;

            }
            OperatorInfo operInfo = new OperatorInfo(operatorType, operType1, operType2, resultType);
            operInfo.method = method;
            var key = GetKey(operType1, operType2);

            if (cachedBinaryOpers == null)
                cachedBinaryOpers = new Dictionary<ExpressionType, Dictionary<Tuple<Type, Type>, OperatorInfo>>(ExpressionType.None.EqualityComparer());

            if (!cachedBinaryOpers.ContainsKey(operatorType))
                cachedBinaryOpers[operatorType] = new Dictionary<Tuple<Type, Type>, OperatorInfo>(/*EqualityComparers.KeyValuePairTypeType*/);

            cachedBinaryOpers[operatorType][key] = operInfo;


        }



        internal OperatorInfo GetOperatorInfo(ExpressionType operatorType, Type operType1, Type operType2)
        {
            OperatorInfo operInfo;
            if (!TryGetOperatorInfo(operatorType, operType1, operType2, out operInfo))
            {
                throw new OperatorNotImplementedException(operatorType, operType1, operType2);
            }
            return operInfo;
        }

        internal bool TryGetOperatorInfo(ExpressionType operatorType, Type operType1, Type operType2, out OperatorInfo operInfo)
        {
            if (cachedBinaryOpers != null)
            {
                Log("TryGetOperatorInfo cachedBinaryOpers not null");
                Dictionary<Tuple<Type, Type>, OperatorInfo> dic;

                if (cachedBinaryOpers.TryGetValue(operatorType, out dic))
                {

                    Log("cachedBinaryOpers.TryGetValue " + operatorType);
                    var t1 = operType1;
                    while (t1 != null)
                    {
                        var t2 = operType2;
                        while (t2 != null)
                        {
                            var key = GetKey(t1, t2);

                            if (dic.TryGetValue(key, out operInfo))
                                return true;

                            key = GetKey(t2, t1);
                            if (dic.TryGetValue(key, out operInfo))
                                return true;
                            t2 = t2.BaseType;
                        }
                        t1 = t1.BaseType;
                    }
                    Log("not found oper:  " + operatorType + " " + operType1 + ", " + operType2);
                }
            }

            if (parent != null)
                return parent.TryGetOperatorInfo(operatorType, operType1, operType2, out operInfo);

            operInfo = null;
            return false;
        }

        internal OperatorInfo GetConvertOperatorInfo(ExpressionType operatorType, Type operType1, Type convertToType)
        {
            OperatorInfo operInfo;
            if (!TryGetConvertOperatorInfo(operatorType, operType1, convertToType, out operInfo))
            {
                throw new OperatorNotImplementedException(operatorType, operType1, convertToType);
            }
            return operInfo;
        }
        internal bool TryGetConvertOperatorInfo(ExpressionType operatorType, Type operType1, Type convertToType, out OperatorInfo operInfo)
        {
            Dictionary<Tuple<Type, Type>, OperatorInfo> dic = null;
            if (cachedBinaryOpers != null)
            {
                Log("TryGetConvertOperatorInfo cachedBinaryOpers not null");
                if (cachedBinaryOpers.TryGetValue(operatorType, out dic))
                {
                    var t1 = operType1;
                    while (t1 != null)
                    {
                        if (dic.TryGetValue(new Tuple<Type, Type>(t1, convertToType), out operInfo))
                        {
                            return true;
                        }
                        t1 = t1.BaseType;
                    }
                }
            }

            if (parent != null)
            {
                if (parent.TryGetConvertOperatorInfo(operatorType, operType1, convertToType, out operInfo))
                    return true;
            }
            if (dic != null)
            {

                //if (dic.TryGetValue(new Tuple<Type, Type>(typeof(object), typeof(Type)), out operInfo))
                //    return true;
            }
            operInfo = null;
            return false;
        }


        internal OperatorInfo GetOperatorInfo(ExpressionType expressionType, Type operType1)
        {
            OperatorInfo operInfo;
            if (!TryGetOperatorInfo(expressionType, operType1, out operInfo))
            {
                throw new OperatorNotImplementedException(expressionType, operType1, null);
            }
            return operInfo;
        }

        internal bool TryGetOperatorInfo(ExpressionType operatorType, Type operType1, out OperatorInfo operInfo)
        {
            if (cachedBinaryOpers != null)
            {
                Dictionary<Tuple<Type, Type>, OperatorInfo> dic;
                if (cachedBinaryOpers.TryGetValue(operatorType, out dic))
                {
                    var t = operType1;
                    while (t != null)
                    {
                        if (dic.TryGetValue(new Tuple<Type, Type>(t, null), out operInfo))
                            return true;
                        t = t.BaseType;
                    }

                }
            }

            if (parent != null)
                return parent.TryGetOperatorInfo(operatorType, operType1, out operInfo);

            operInfo = null;
            return false;
        }

        private Tuple<Type, Type> GetKey(Type oper1, Type oper2)
        {
            return new Tuple<Type, Type>(oper1, oper2);
        }

        public void AddVariable<T>(string name)
            => AddVariable(typeof(T), name);

        public void AddVariable(Type type, string name)
        {
            if (variables == null)
                variables = new Dictionary<string, VariableInfo>();
            VariableInfo variable = new VariableInfo(type, name, null);
            variables[name] = variable;
        }


        public bool ContainsVariable(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            CompileContext current = this;
            while (current != null)
            {
                if (current.variables != null && current.variables.ContainsKey(name))
                    return true;
                if (current.context != null && current.context.ContainsVariable(name))
                    return true;
                current = current.parent;
            }
            return false;
        }

        public Type GetVariableType(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            CompileContext current = this;
            VariableInfo variable;

            while (current != null)
            {
                if (current.variables != null && current.variables.TryGetValue(name, out variable))
                    return variable.Type;

                if (current.context != null && current.context.ContainsVariable(name))
                    return current.context.GetVariableType(name);

                current = current.parent;
            }

            throw new VariableException(Resource1.VariableNotFound, name);
        }

        public InvocationDelegate Compile(Expression expression)
        {
            var child = new CompileContext(this);
            CompiledDelegate eval;
            using (child.BeginScope())
            {
                eval = expression.Compile(this);
            }

            if (expression is ConstantExpression)
                return (ctx) => eval(new InvocationContext(ctx));

            return (ctx) =>
            {
                InvocationContext invoke;

                invoke = new InvocationContext(ctx);

                var ret = eval(invoke);

                if (invoke.GotoType == GotoType.Return)
                    return invoke.GotoValue;
                return ret;
            };
        }

        public IDisposable BeginScope()
        {
            return new Scope(this);
        }


        class Scope : IDisposable
        {
            private CompileContext origin;
            private bool isDisposed;

            public Scope(CompileContext newScope)
            {
                System.Threading.Monitor.Enter(lockObj);
                origin = current;
                current = newScope;

            }

            public void Dispose()
            {

                if (isDisposed)
                    return;
                current = origin;
                origin = null;
                isDisposed = true;
                System.Threading.Monitor.Exit(lockObj);
            }
            ~Scope()
            {
                Dispose();
            }
        }
    }
}
