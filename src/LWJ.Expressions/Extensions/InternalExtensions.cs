using System;
using System.Reflection;

namespace LWJ.Expressions
{
    internal static class InternalExtensions
    {
        public static readonly object[] EmptyObjects = new object[0];

        public static string FormatArgs(this string source, params object[] args)
        {
            return string.Format(source, args);
        }
        public static T GetCustomAttribute<T>(this ICustomAttributeProvider member, bool inherit)
        where T : Attribute
        {
            var attrs = member.GetCustomAttributes(typeof(T), inherit);
            if (attrs != null && attrs.Length > 0)
                return (T)attrs[0];
            return null;
        }


        public static bool IsParamArray(this MethodBase method)
        {
            var ps = method.GetParameters();
            if (ps.Length > 0 && ps[ps.Length - 1].IsDefined(typeof(ParamArrayAttribute), false))
                return true;
            return false;
        }

        public static object[] AlignParamArrayArguments(this MethodBase method, object[] args)
        {
            var ps = method.GetParameters();
            if (!IsParamArray(method))
                return args;
            int length = ps.Length;
            var last = ps[ps.Length - 1];

            if (args.Length + 1 == length)
            {
                var array = Array.CreateInstance(last.ParameterType.GetElementType(), 0);
                var newArgs = new object[length];
                Array.Copy(args, newArgs, length - 1);
                newArgs[length - 1] = array;
                return newArgs;
            }
            else if (args.Length > length || last.ParameterType != args[length - 1].GetType())
            {
                var array = Array.CreateInstance(last.ParameterType.GetElementType(), args.Length - length + 1);

                for (int i = 0; i < array.Length; i++)
                {
                    array.SetValue(args[last.Position + i], i);
                }
                var newArgs = new object[length];
                Array.Copy(args, newArgs, length - 1);
                newArgs[length - 1] = array;
                return newArgs;
            }

            return args;
        }


    }
}
