﻿using System.Xml;

namespace LWJ.Expressions.Xml
{
    public class DuplicateNodeException : ExpressionReadException
    {
        public DuplicateNodeException(XmlNode node)
               : this(null, node)
        {
        }
        public DuplicateNodeException(string message, XmlNode node)
            : base(message ?? Resource1.Read_DuplicateNode, node)
        {
        }
    }
}
