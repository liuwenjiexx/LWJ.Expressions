using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Linq;

namespace LWJ.Expressions
{


    /// <summary>
    /// expression base class
    /// </summary>
    public abstract class Expression
    {
        private ExpressionType expressionType;
        private Type valueType;
        public static readonly Expression[] EmptyExpressions = new Expression[0];


        public static readonly ConstantExpression True = new ConstantExpression(true, typeof(bool));
        public static readonly ConstantExpression False = new ConstantExpression(false, typeof(bool));
        public static readonly ConstantExpression Null = new ConstantExpression(null, typeof(object));
        public static readonly ConstantExpression Undefined = new ConstantExpression(null, typeof(object));

        protected static readonly ReadOnlyCollection<ParameterExpression> ReadOnlyEmptyVariables = new ReadOnlyCollection<ParameterExpression>(new ParameterExpression[0]);
        protected static readonly ReadOnlyCollection<Expression> ReadOnlyEmptyExpressions = new ReadOnlyCollection<Expression>(new Expression[0]);


        protected Expression()
        {

        }
        protected Expression(ExpressionType expressionType)
        {
            this.expressionType = expressionType;
            this.valueType = typeof(void);
        }

        protected Expression(ExpressionType expressionType, Type valueType)
        {
            this.expressionType = expressionType;
            this.valueType = valueType ?? throw new ArgumentNullException(nameof(valueType));
        }

        public virtual ExpressionType ExpressionType => expressionType;

        public virtual Type ValueType => valueType;

        public abstract CompiledDelegate Compile(CompileContext ctx);

        static CompileContext CurrentOrDefault => CompileContext.Current ?? CompileContext.Default;

        static void ValidateUnary(Expression operand, MethodInfo method)
        {
            if (operand == null) throw new ArgumentNullException(nameof(operand));
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (!method.IsStatic) throw new ArgumentException(Resource1.MethodNotStatic, nameof(method));
            if (method.GetParameters().Length != 1) throw new ArgumentException(Resource1.MethodNotArgs2, nameof(method));
        }

        static void ValidateBinary(Expression left, Expression right, MethodInfo method, bool isEvalBefore = false)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (!method.IsStatic) throw new ArgumentException(Resource1.MethodNotStatic, nameof(method));
            if (isEvalBefore)
            {
                if (method.GetParameters().Length != 3) throw new ArgumentException(Resource1.MethodNotArgs2, nameof(method));
            }
            else
            {
                if (method.GetParameters().Length != 2) throw new ArgumentException(Resource1.MethodNotArgs2, nameof(method));
            }
        }

        public static BinaryExpression Add(Expression left, Expression right) =>
            Add(left, right, GetImplementMethod(ExpressionType.Add, left, right));
        public static BinaryExpression Add(Expression left, Expression right, MethodInfo method)
        {
            ValidateBinary(left, right, method);
            return new BinaryExpression(ExpressionType.Add, left, right, method);
        }
        public static BinaryExpression Subtract(Expression left, Expression right) =>
             Subtract(left, right, GetImplementMethod(ExpressionType.Subtract, left, right));

        public static BinaryExpression Subtract(Expression left, Expression right, MethodInfo method)
        {
            ValidateBinary(left, right, method);
            return new BinaryExpression(ExpressionType.Subtract, left, right, method);
        }
        public static BinaryExpression Multiply(Expression left, Expression right) =>
             Multiply(left, right, GetImplementMethod(ExpressionType.Multiply, left, right));

        public static BinaryExpression Multiply(Expression left, Expression right, MethodInfo method)
        {
            ValidateBinary(left, right, method);
            return new BinaryExpression(ExpressionType.Multiply, left, right, method);
        }
        public static BinaryExpression Divide(Expression left, Expression right) =>
            Divide(left, right, GetImplementMethod(ExpressionType.Divide, left, right));
        public static BinaryExpression Divide(Expression left, Expression right, MethodInfo method)
        {
            ValidateBinary(left, right, method);
            return new BinaryExpression(ExpressionType.Divide, left, right, method);
        }
        public static BinaryExpression Modulo(Expression left, Expression right) =>
             Modulo(left, right, GetImplementMethod(ExpressionType.Modulo, left, right));

        public static BinaryExpression Modulo(Expression left, Expression right, MethodInfo method)
        {
            ValidateBinary(left, right, method);
            return new BinaryExpression(ExpressionType.Modulo, left, right, method);
        }

        public static UnaryExpression PreIncrement(AccessableExpression left) =>
            new AddAssignExpression(ExpressionType.PreIncrement, left, Constant(1));

        public static UnaryExpression PreDecrement(AccessableExpression left) =>
            new AddAssignExpression(ExpressionType.PreDecrement, left, Constant(-1));


        public static UnaryExpression PostIncrement(AccessableExpression left) =>
            new AddAssignExpression(ExpressionType.PostIncrement, left, Constant(1));

        public static UnaryExpression PostDecrement(AccessableExpression left) =>
            new AddAssignExpression(ExpressionType.PostDecrement, left, Constant(-1));


        public static UnaryExpression Negate(Expression left) =>
              Negate(left, GetImplementMethod(ExpressionType.Negate, left));
        public static UnaryExpression Negate(Expression left, MethodInfo method) =>
            new UnaryExpression(ExpressionType.Negate, left, method);

        public static BinaryExpression AddAssign(AccessableExpression left, Expression right) =>
            new AssignExpression(ExpressionType.AddAssign, left, Add(left, right));

        public static BinaryExpression SubtractAssign(AccessableExpression left, Expression right) =>
            new AssignExpression(ExpressionType.SubtractAssign, left, Subtract(left, right));

        public static BinaryExpression MultiplyAssign(AccessableExpression left, Expression right) =>
            new AssignExpression(ExpressionType.MultiplyAssign, left, Multiply(left, right));

        public static BinaryExpression DivideAssign(AccessableExpression left, Expression right) =>
            new AssignExpression(ExpressionType.DivideAssign, left, Divide(left, right));

        public static BinaryExpression ModuloAssign(AccessableExpression left, Expression right) =>
            new AssignExpression(ExpressionType.ModuloAssign, left, Modulo(left, right));

        public static BlockExpression Block(IEnumerable<ParameterExpression> variables, IEnumerable<Expression> expressions) =>
            new BlockExpression(variables, expressions);

        public static BlockExpression Block(IEnumerable<ParameterExpression> variables, params Expression[] expressions) =>
            new BlockExpression(variables, expressions);

        public static BlockExpression Block(IEnumerable<Expression> expressions) =>
            new BlockExpression(null, expressions);

        public static BlockExpression Block(params Expression[] expressions) =>
            new BlockExpression(null, expressions);

        private static bool IsConstantType(Type valueType)
        {
            if (valueType.IsPrimitive)
                return true;
            if (valueType == typeof(DateTime))
                return true;
            //if (valueType == typeof(Type))
            //    return true;
            //if (valueType.GetType() == typeof(Type).GetType())
            //if (valueType.MemberType == MemberTypes.TypeInfo)
            //    return true;
            TypeCode typeCode = Type.GetTypeCode(valueType);
            switch (typeCode)
            {
                case TypeCode.String:
                    return true;
            }
            return false;
        }

        public static ConstantExpression Constant(object value)
        {
            if (value == null)
                return ConstantExpression.Null;
            if (!typeof(Type).IsInstanceOfType(value))
            {
                Type valueType = value.GetType();
                if (!IsConstantType(valueType))
                    throw new ArgumentException(Resource1.InvalidConstantType, nameof(value));
            }
            return new ConstantExpression(value, value.GetType());
        }
        public static ConstantExpression Constant(object value, Type valueType)
        {
            if (valueType == null) throw new ArgumentNullException(nameof(valueType));

            if (!typeof(Type).IsInstanceOfType(value) && !IsConstantType(valueType))
                throw new ArgumentException(Resource1.InvalidConstantType, nameof(valueType));
            return new ConstantExpression(value, valueType);
        }

        public static ConstantExpression Constant<T>(T value)
            => Constant(value, typeof(T));


        public static ParameterExpression Variable<T>(string name)
            => Variable(typeof(T), name);

        public static ParameterExpression Variable(Type type, string name)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (name == null) throw new ArgumentNullException(nameof(name));
            return new ParameterExpression(type, name);
        }
        /// <summary>
        /// call static method
        /// </summary>
        public static MethodCallExpression Call(MethodInfo method, IEnumerable<Expression> arguments)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (!method.IsStatic) throw new ArgumentException(Resource1.MethodNotStatic, nameof(method));
            return new MethodCallExpression(null, method, arguments);
        }
        /// <summary>
        /// call static method
        /// </summary>
        public static MethodCallExpression Call(MethodInfo method, params Expression[] arguments)
            => Call(method, (IEnumerable<Expression>)arguments);

        /// <summary>
        /// call static method
        /// </summary>
        public static MethodCallExpression Call(Type type, string methodName, IEnumerable<Expression> arguments)
            => Call(type, methodName, Type.EmptyTypes, arguments);
        /// <summary>
        /// call static method
        /// </summary>
        public static MethodCallExpression Call(Type type, string methodName, params Expression[] arguments)
            => Call(type, methodName, Type.EmptyTypes, (IEnumerable<Expression>)arguments);

        /// <summary>
        /// call static method
        /// </summary>
        public static MethodCallExpression Call(Type type, string methodName, Type[] argTypes, IEnumerable<Expression> arguments)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));
            Type objType = type;
            Type[] methodArgTypes = argTypes;

            int argLength = arguments == null ? 0 : arguments.Count();
            if (argLength > 0 && (methodArgTypes == null || methodArgTypes.Length == 0))
            {
                methodArgTypes = arguments.Select(o => o.ValueType).ToArray();
            }
            var mInfo = objType.GetMethod(methodName, methodArgTypes);
            if (mInfo == null)
                throw new MissingMethodException(objType.FullName, methodName);

            return Call(mInfo, arguments);
        }

        /// <summary>
        /// call static method
        /// </summary>
        public static MethodCallExpression Call(Type type, string methodName, Type[] argTypes, Expression[] arguments)
        => Call(type, methodName, argTypes, (IEnumerable<Expression>)arguments);



        /// <summary>
        /// call method
        /// </summary>
        public static MethodCallExpression Call(Expression instance, MethodInfo method, params Expression[] arguments) =>
             Call(instance, method, (IEnumerable<Expression>)arguments);
        /// <summary>
        /// call method
        /// </summary>
        public static MethodCallExpression Call(Expression instance, MethodInfo method, IEnumerable<Expression> arguments)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (method == null) throw new ArgumentNullException(nameof(method));
            //if (method.IsStatic) throw new ArgumentException(Resource1.MethodIsStatic, nameof(method));
            return new MethodCallExpression(instance, method, arguments);
        }
        /// <summary>
        /// call method
        /// </summary>
        public static MethodCallExpression Call(Expression instance, string methodName, params Expression[] arguments)
            => Call(instance, null, methodName, Type.EmptyTypes, arguments);
        /// <summary>
        /// call method
        /// </summary>
        public static MethodCallExpression Call(Expression instance, string methodName, IEnumerable<Expression> arguments)
            => Call(instance, null, methodName, Type.EmptyTypes, arguments);
        /// <summary>
        /// call method
        /// </summary>
        public static MethodCallExpression Call(Expression instance, Type type, string methodName, Type[] argTypes, IEnumerable<Expression> arguments)
            => Call(instance, type, methodName, Type.EmptyTypes, arguments.ToArray());
        /// <summary>
        /// call method
        /// </summary>
        public static MethodCallExpression Call(Expression instance, Type type, string methodName, Type[] argTypes, params Expression[] arguments)
        {
            instance = instance ?? Null;
            //if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));

            Type objType = type;
            Type[] methodArgTypes = argTypes;
            if (objType == null)
                objType = instance.ValueType;
            if (objType == null)//void
                throw new ArgumentNullException(nameof(type));
            int argLength = arguments.Length;
            if (argLength > 0 && (methodArgTypes == null || methodArgTypes.Length == 0))
            {
                methodArgTypes = arguments.Select(o => o.ValueType).ToArray();
            }
            var mInfo = objType.GetMethod(methodName, methodArgTypes);
            if (mInfo == null)
            {
                foreach (var m in objType.GetMethods())
                {
                    if (m.Name == methodName)
                    {

                    }
                }
                if (mInfo == null)
                    throw new MissingMethodException(objType.FullName, methodName);
            }
            return Call(instance, mInfo, arguments);
        }



        /// <summary>
        /// call function or delegate
        /// </summary>
        public static FunctionCallExpression Call(Expression function, Type returnType, IEnumerable<Expression> arguments)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));
            return new FunctionCallExpression(function, returnType, arguments);
        }
        /// <summary>
        /// call function or delegate
        /// </summary>
        public static FunctionCallExpression Call(Expression function, Type returnType, params Expression[] arguments)
            => Call(function, returnType, (IEnumerable<Expression>)arguments);
        /// <summary>
        /// call function or delegate
        /// </summary>
        public static FunctionCallExpression Call(string functionName, Type returnType, IEnumerable<Expression> arguments)
            => Call(Variable<Delegate>(functionName), returnType, arguments);
        /// <summary>
        /// call function or delegate
        /// </summary>
        public static FunctionCallExpression Call(string functionName, Type returnType, params Expression[] arguments)
            => Call(Variable<Delegate>(functionName), returnType, (IEnumerable<Expression>)arguments);


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


        public static MemberExpression Field(Expression instance, FieldInfo field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));
            if (!field.IsStatic && instance == null) throw new ArgumentNullException(nameof(instance));

            return new MemberExpression(instance, field.FieldType, field, false);
        }
        public static MemberExpression Field(Expression instance, string fieldName)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            return Field(instance, null, fieldName);
        }
        /// <summary>
        /// static field
        /// </summary>
        public static MemberExpression Field(Type type, string fieldName)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return Field(null, type, fieldName);
        }

        public static MemberExpression Field(Expression instance, Type type, string fieldName)
        {
            if (fieldName == null) throw new ArgumentNullException(nameof(fieldName));
            BindingFlags flags;
            flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.GetField | BindingFlags.SetField;
            var mInfo = GetMember(instance, type, fieldName, flags);
            if (mInfo == null) throw new MissingFieldException(type.FullName, fieldName);
            return new MemberExpression(instance, type, mInfo, true);
        }


        public static MemberExpression Property(Expression instance, PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            if (property.CanRead && !property.GetGetMethod().IsStatic && instance == null) throw new ArgumentNullException("Property Not Is Static", nameof(instance));
            if (property.CanWrite && !property.GetSetMethod().IsStatic && instance == null) throw new ArgumentNullException("Property Not Is Static", nameof(instance));

            return new MemberExpression(instance, property.DeclaringType, property, true);
        }
        public static MemberExpression Property(Expression instance, string propertyName)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            return Property(instance, null, propertyName);
        }
        /// <summary>
        /// static property
        /// </summary>
        public static MemberExpression Property(Type type, string propertyName)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return Property(null, type, propertyName);
        }
        public static MemberExpression Property(Expression instance, Type type, string propertyName)
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
            BindingFlags flags;
            flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.SetProperty;
            var mInfo = GetMember(instance, type, propertyName, flags);
            if (mInfo == null) throw new MissingMemberException(type.FullName, propertyName);
            return new MemberExpression(instance, type, mInfo, true);
        }

        public static MemberExpression PropertyOrField(Type type, string propertyOrField)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return PropertyOrField(null, type, propertyOrField);
        }
        public static MemberExpression PropertyOrField(Expression instance, string propertyOrField)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            return PropertyOrField(instance, null, propertyOrField);
        }
        public static MemberExpression PropertyOrField(Expression instance, Type type, string propertyOrField)
        {
            if (propertyOrField == null) throw new ArgumentNullException(nameof(propertyOrField));
            BindingFlags flags;
            flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.SetField | BindingFlags.SetProperty;
            var mInfo = GetMember(instance, type, propertyOrField, flags);
            if (mInfo == null) throw new MissingMemberException(type.FullName, propertyOrField);

            return new MemberExpression(instance, type, mInfo, null);
        }

        public static MemberExpression Member(Expression instance, Type type, string memberName)
        {
            if (memberName == null) throw new ArgumentNullException(nameof(memberName));

            Type objType = type;
            if (objType == null)
                objType = instance.ValueType;
            if (objType == null)
                throw new ArgumentNullException(nameof(type));
            var mInfos = objType.GetMember(memberName);
            if (mInfos == null || mInfos.Length == 0) throw new MissingMemberException(type.FullName, memberName);

            return new MemberExpression(instance, type, mInfos[0], null);
        }

        public static AssignExpression Assign(AccessableExpression left, Expression right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            return new AssignExpression(left, right);
        }

        public static BinaryExpression Equal(Expression left, Expression right)
            => Equal(left, right, GetImplementMethod(ExpressionType.Equal, left, right));
        public static BinaryExpression Equal(Expression left, Expression right, MethodInfo method)
        {
            ValidateBinary(left, right, method);
            return new BinaryExpression(ExpressionType.Equal, left, right, method);
        }
        public static BinaryExpression NotEqual(Expression left, Expression right)
            => NotEqual(left, right, GetImplementMethod(ExpressionType.NotEqual, left, right));
        public static BinaryExpression NotEqual(Expression left, Expression right, MethodInfo method)
        {
            ValidateBinary(left, right, method);
            return new BinaryExpression(ExpressionType.NotEqual, left, right, method);
        }
        public static BinaryExpression LessThan(Expression left, Expression right)
            => LessThan(left, right, GetImplementMethod(ExpressionType.LessThan, left, right));
        public static BinaryExpression LessThan(Expression left, Expression right, MethodInfo method)
        {
            ValidateBinary(left, right, method);
            return new BinaryExpression(ExpressionType.LessThan, left, right, method);
        }
        public static BinaryExpression LessThanOrEqual(Expression left, Expression right)
            => LessThanOrEqual(left, right, GetImplementMethod(ExpressionType.LessThanOrEqual, left, right));
        public static BinaryExpression LessThanOrEqual(Expression left, Expression right, MethodInfo method)
        {
            ValidateBinary(left, right, method);
            return new BinaryExpression(ExpressionType.LessThanOrEqual, left, right, method);
        }
        public static BinaryExpression GreaterThan(Expression left, Expression right)
            => GreaterThan(left, right, GetImplementMethod(ExpressionType.GreaterThan, left, right));
        public static BinaryExpression GreaterThan(Expression left, Expression right, MethodInfo method)
        {
            ValidateBinary(left, right, method);
            return new BinaryExpression(ExpressionType.GreaterThan, left, right, method);
        }
        public static BinaryExpression GreaterThanOrEqual(Expression left, Expression right)
            => GreaterThanOrEqual(left, right, GetImplementMethod(ExpressionType.GreaterThanOrEqual, left, right));
        public static BinaryExpression GreaterThanOrEqual(Expression left, Expression right, MethodInfo method)
        {
            ValidateBinary(left, right, method);
            return new BinaryExpression(ExpressionType.GreaterThanOrEqual, left, right, method);
        }
        public static BinaryExpression And(Expression left, Expression right)
        {
            var method = typeof(DefaultOperators).GetMethod("And");
            ValidateBinary(left, right, method, true);
            return new BinaryExpression(ExpressionType.And, left, right, method, true);
        }
        public static BinaryExpression And(Expression left, Expression right, MethodInfo method)
        {
            ValidateBinary(left, right, method, true);
            return new BinaryExpression(ExpressionType.And, left, right, method);
        }
        public static BinaryExpression Or(Expression left, Expression right)
        {
            var method = typeof(DefaultOperators).GetMethod("Or");
            ValidateBinary(left, right, method, true);
            return new BinaryExpression(ExpressionType.Or, left, right, method, true);
        }
        public static BinaryExpression Or(Expression left, Expression right, MethodInfo method)
        {
            ValidateBinary(left, right, method, true);
            return new BinaryExpression(ExpressionType.Or, left, right, method);
        }
        public static UnaryExpression Not(Expression expression)
            => new UnaryExpression(ExpressionType.Not, expression, GetImplementMethod(ExpressionType.Not, expression));

        public static UnaryExpression Not(Expression expression, MethodInfo method)
        {
            ValidateUnary(expression, method);
            return new UnaryExpression(ExpressionType.Not, expression, method);
        }

        public static ConditionalExpression If(Expression test, Expression ifTrue) =>
            new ConditionalExpression(test, ifTrue, null);

        public static ConditionalExpression If(Expression test, Expression ifTrue, Expression ifFalse)
        {
            if (test == null) throw new ArgumentNullException(nameof(test));
            return new ConditionalExpression(test, ifTrue, ifFalse);
        }

        public static SwitchExpression Switch(Expression testValue, IEnumerable<SwitchCase> cases)
            => Switch(testValue, null, cases);
        public static SwitchExpression Switch(Expression testValue, params SwitchCase[] cases)
            => Switch(testValue, null, cases);
        public static SwitchExpression Switch(Expression testValue, Expression defaultBody, IEnumerable<SwitchCase> cases)
        {
            if (testValue == null) throw new ArgumentNullException(nameof(testValue));
            return new SwitchExpression(testValue, defaultBody, cases);
        }
        public static SwitchExpression Switch(Expression testValue, Expression defaultBody, params SwitchCase[] cases)
            => Switch(testValue, defaultBody, (IEnumerable<SwitchCase>)cases);

        public static LoopExpression Loop(Expression body)
        {
            if (body == null)
                throw new ArgumentNullException(nameof(body));
            return new LoopExpression(body);
        }


        public static GotoExpression Break()
            => new GotoExpression(GotoType.Break, Null);

        public static GotoExpression Continue()
            => new GotoExpression(GotoType.Continue, Null);

        //public static GotoExpression Goto()
        //{
        //    return null;
        //}

        public static GotoExpression Return()
            => Return(Null);

        public static GotoExpression Return(Expression value)
            => new GotoExpression(GotoType.Return, value);


        public static TypeUnaryExpression Convert(Expression expression, Type convertToType)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            if (convertToType == null) throw new ArgumentNullException(nameof(convertToType));
            var method = CurrentOrDefault.GetConvertOperatorInfo(ExpressionType.Convert, expression.ValueType, convertToType).method;
            return Convert(expression, convertToType, method);
        }

        public static TypeUnaryExpression Convert(Expression expression, Type convertToType, MethodInfo method)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            if (convertToType == null) throw new ArgumentNullException(nameof(convertToType));
            return new TypeUnaryExpression(ExpressionType.Convert, expression, convertToType, method);
        }

        public static TypeBinaryExpresion TypeAs(Expression value, Type asType)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (asType == null) throw new ArgumentNullException(nameof(asType));
            if (asType.IsValueType) throw new ArgumentException(Resource1.TypeAsNotValueType, nameof(asType));

            return new TypeBinaryExpresion(ExpressionType.TypeAs, value, asType);
        }

        public static TypeBinaryExpresion TypeIs(Expression value, Type isType)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (isType == null) throw new ArgumentNullException(nameof(isType));
            return new TypeBinaryExpresion(ExpressionType.TypeIs, value, isType);
        }

        public static FunctionExpression Function(string name, Expression body)
            => Function(name, null, body, typeof(void));

        public static FunctionExpression Function(string name, Expression body, Type returnType)
            => Function(name, null, body, returnType);

        public static FunctionExpression Function(string name, IEnumerable<ParameterExpression> arguments, Expression body)
            => Function(name, arguments, body, typeof(void));

        public static FunctionExpression Function(string name, IEnumerable<ParameterExpression> arguments, Expression body, Type returnType)
        {
            if (body == null) throw new ArgumentNullException(nameof(body));
            return new FunctionExpression(name, arguments, body, returnType);
        }

        public static Type GetFuncType(params Type[] typeArgs)
        {
            if (typeArgs.Length == 0 || typeArgs.Length > 5)
                throw new ArgumentOutOfRangeException(nameof(typeArgs));
            Type type;
            //mono not support
            //type= Type.GetType("System.Func`" + typeArgs.Length);
            switch (typeArgs.Length)
            {
                case 1:
                    type = typeof(Func<>);
                    break;
                case 2:
                    type = typeof(Func<,>);
                    break;
                case 3:
                    type = typeof(Func<,,>);
                    break;
                case 4:
                    type = typeof(Func<,,,>);
                    break;
                case 5:
                default:
                    type = typeof(Func<,,,,>);
                    break;
            }
            return type.MakeGenericType(typeArgs);
        }

        public static Type GetActionType(params Type[] typeArgs)
        {
            if (typeArgs == null || typeArgs.Length == 0)
                return typeof(Action);

            if (typeArgs.Length > 4)
                throw new ArgumentOutOfRangeException(nameof(typeArgs));

            Type type;
            //type = Type.GetType("System.Action`" + typeArgs.Length);
            switch (typeArgs.Length)
            {
                case 1:
                    type = typeof(Action<>);
                    break;
                case 2:
                    type = typeof(Action<,>);
                    break;
                case 3:
                    type = typeof(Action<,,>);
                    break;
                case 4:
                default:
                    type = typeof(Action<,,,>);
                    break;

            }
            return type.MakeGenericType(typeArgs);
        }



        internal static MethodInfo GetImplementMethod(ExpressionType operatorType, Expression oper1)
        {
            return CurrentOrDefault.GetOperatorInfo(operatorType, oper1.ValueType).method;
        }
        internal static MethodInfo GetImplementMethod(ExpressionType operatorType, Expression oper1, Expression oper2)
        {
            return GetImplementMethod(operatorType, oper1.ValueType, oper2.ValueType);
        }
        internal static MethodInfo GetImplementMethod(ExpressionType operatorType, Type operType1, Type operType2)
        {
            return CurrentOrDefault.GetOperatorInfo(operatorType, operType1, operType2).method;
        }
        public static GroupExpression Group(Expression expr)
        {
            return new GroupExpression(expr);
        }
    }

    public delegate object CompiledDelegate(InvocationContext invoke);

    public delegate object InvocationDelegate(IExpressionContext ctx);


}