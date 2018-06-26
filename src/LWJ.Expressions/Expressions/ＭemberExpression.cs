/**************************************************************
 *  Filename:    ＭemberExpression.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/
using System;
using System.Reflection;


namespace LWJ.Expressions
{

    /// <summary>
    /// field or property access
    /// </summary>
    public class MemberExpression : AccessableExpression
    {
        private MemberInfo member;
        private Expression instance;
        //private Type valueType;
        //private string memberName;
        private bool? isProperty;
        private Type classType;


        internal MemberExpression(Expression instance, Type classType, MemberInfo member, bool? isProperty)
            : base(ExpressionType.Member, member is PropertyInfo ? ((PropertyInfo)member).PropertyType : member is FieldInfo ? ((FieldInfo)member).FieldType : typeof(object))
        {
            this.instance = instance ?? Expression.Null;
            this.classType = classType;
            this.member = member;
            //this.valueType = valueType;
            this.isProperty = isProperty;
        }

        //internal ＭemberExpression(Expression instance, Type instanceType, string memberName, bool? isProperty)
        //{
        //    this.instance = instance;
        //    this.instanceType = instanceType;
        //    this.memberName = memberName;
        //    this.isProperty = isProperty;
        //}

        public Expression Instance => instance;

        public Type ClassType => classType;

        public MemberInfo Member => member;

        //public override Type ValueType => valueType;


        public bool IsPropertyOrField => isProperty == null;

        public bool IsProperty => isProperty.HasValue && isProperty.Value;

        //private MemberInfo GetMember()
        //{
        //    if (member != null)
        //        return member;
        //    Type memberType = this.instanceType;
        //    if (memberType == null)
        //    {
        //        if (instance == null)
        //            throw new ExpressionException("Membr Instance Null. Member: {0}".FormatArgs(memberName));
        //        memberType = instance.ValueType;
        //        if (memberType == null)
        //            throw new ExpressionException("Membr Type Null. Member: {0}".FormatArgs(memberName));
        //    }

        //    if (isProperty == null || isProperty.Value)
        //    {
        //        PropertyInfo pInfo = memberType.GetProperty(memberName);
        //        if (pInfo != null)
        //            return pInfo;
        //        if (isProperty != null && pInfo == null) throw new ExpressionException(Resource1.PropertyNotFound.FormatArgs(memberName));
        //    }

        //    if (isProperty == null || !isProperty.Value)
        //    {
        //        FieldInfo fInfo = memberType.GetField(memberName);
        //        if (fInfo != null)
        //            return fInfo;
        //        if (isProperty != null && fInfo == null) throw new ExpressionException(Resource1.FieldNotFound.FormatArgs(memberName));
        //    }

        //    throw new ExpressionException(Resource1.PropertyOrFieldNotFound.FormatArgs(memberName));
        //}

        private CompiledDelegate CompileTarget(CompileContext ctx)
        {
            if (instance == null)
                throw new MemberAccessException("Target Null");
            var targetEval = instance.Compile(ctx);
            return targetEval;
        }

        public override CompiledDelegate Compile(CompileContext ctx)
        {
            var property = member as PropertyInfo;
            CompiledDelegate targetEval;

            if (property != null)
            {
                var getter = property.GetGetMethod();
                if (getter == null)
                    throw new MemberAccessException(Resource1.PropertyNotReadable.FormatArgs(member.Name));
                if (getter.IsStatic)
                    return (invoke) => getter.Invoke(null, InternalExtensions.EmptyObjects);
                else
                {

                    targetEval = CompileTarget(ctx);
                    return (invoke) => getter.Invoke(targetEval(invoke), InternalExtensions.EmptyObjects);
                }
            }



            var field = member as FieldInfo;
            if (field.IsStatic)
            {
                return (invoke) => field.GetValue(null);
            }
            else
            {
                targetEval = CompileTarget(ctx);
                return (invoke) => field.GetValue(targetEval(invoke));
            }
        }

        public override Action<InvocationContext, object> CompileSetValue(CompileContext ctx)
        {

            CompiledDelegate targetEval;

            var property = member as PropertyInfo;
            if (property != null)
            {
                var setter = property.GetSetMethod();
                if (setter == null)
                    throw new MemberAccessException(Resource1.PropertyNotWritable.FormatArgs(member.Name));

                if (setter.IsStatic)
                {
                    return (invoke, value) =>
                    {
                        setter.Invoke(null, new object[] { value });
                    };
                }
                else
                {
                    targetEval = CompileTarget(ctx);
                    return (invoke, value) =>
                    {
                        setter.Invoke(targetEval(invoke), new object[] { value });
                    };
                }
            }


            var field = member as FieldInfo;

            if (field.IsInitOnly)
                throw new MemberAccessException(Resource1.FieldNotWritable.FormatArgs(member.Name));
            if (field.IsStatic)
            {
                return (invoke, value) =>
                {
                    field.SetValue(null, value);
                };
            }
            else
            {
                targetEval = CompileTarget(ctx);
                return (invoke, value) =>
                {
                    var obj = targetEval(invoke);
                    field.SetValue(obj, value);
                };
            }
        }

    }
}
