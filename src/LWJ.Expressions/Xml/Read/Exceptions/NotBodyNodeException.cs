using System.Xml;

namespace LWJ.Expressions.Xml
{
    public class NotBodyNodeException : ExpressionReadException
    {
        public NotBodyNodeException(XmlNode node)
               : this(null, node)
        {
        }
        public NotBodyNodeException(string message, XmlNode node)
            : base(message ?? Resource1.Read_NotBodyNode, node)
        {
        }
    }
}
