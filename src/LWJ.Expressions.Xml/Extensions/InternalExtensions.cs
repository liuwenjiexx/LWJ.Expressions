namespace LWJ.Expressions.Xml
{
    internal static partial class InternalExtensions
    {
        public static string FormatArgs(this string source, params object[] args)
        {
            return string.Format(source, args);
        }

    }
}
