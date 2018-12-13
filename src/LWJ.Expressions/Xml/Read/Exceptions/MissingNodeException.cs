using System;
using System.Xml;

namespace LWJ.Expressions.Xml
{
    public class MissingNodeException : ExpressionReadException
    {
        public MissingNodeException(XmlNode node, string childNodeName)
            : this(null, node, childNodeName)
        {

        }

        public MissingNodeException(string message, XmlNode node, string childNodeName)
            : base(message ?? Resource1.Parse_MissingChildNode, node)
        {
            this.ChildNodeName = childNodeName;
        }

        public string ChildNodeName { get; private set; }

        public override string Message
        {
            get
            {
                string message = base.Message;
                if (!string.IsNullOrEmpty(ChildNodeName))
                    message += Environment.NewLine + Resource1.Parse_ChildNodeName.FormatArgs(ChildNodeName);
                return message;
            }
        }

    }
}
