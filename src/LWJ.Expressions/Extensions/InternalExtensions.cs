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

        
             
        

    }
}
