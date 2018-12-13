using System.Xml;

namespace LWJ.Expressions.Xml
{
    public class UnknowNodeNameException : ExpressionReadException
    {
        public UnknowNodeNameException(XmlNode node)
               : this(null, node)
        {
        }

        public UnknowNodeNameException(string message, XmlNode node)
            : base(message ?? Resource1.Parse_InvalidNodeName, node)
        {
        }

    }
}
