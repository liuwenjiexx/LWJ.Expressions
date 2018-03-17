/**************************************************************
 *  Filename:    ExpressionReadException.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/
using System;
using System.Xml;

namespace LWJ.Expressions.Xml
{
    public class ExpressionReadException : Exception
    {
        public ExpressionReadException(string message, XmlNode node, Exception innerException = null)
            : base(message, innerException)
        {
            this.Node = node;
            if (node != null)
            {
                NodeName = node.LocalName;
                NodeNamespace = node.NamespaceURI;
            }
        }


        public XmlNode Node { get; private set; }

        public string NodeName { get; private set; }

        public string NodeNamespace { get; private set; }

        public override string Message
        {
            get
            {
                string message = base.Message;
                if (!string.IsNullOrEmpty(NodeName))
                    message += Environment.NewLine + Resource1.Parse_NodeName.FormatArgs(NodeName);
                if (!string.IsNullOrEmpty(NodeNamespace))
                    message += Environment.NewLine + Resource1.Read_NodeNamespace.FormatArgs(NodeNamespace);
                return message;
            }
        }
    }
}
