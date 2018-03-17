/**************************************************************
 *  Filename:    InvalidConvertReadException.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/
using System;
using System.Xml;

namespace LWJ.Expressions.Xml
{
    public class InvalidConvertReadException : ExpressionReadException
    {
        public InvalidConvertReadException(XmlNode node, string parseText, Type convertToType, Exception innerException = null)
               : this(null, node, parseText, convertToType, innerException)
        {
        }

        public InvalidConvertReadException(string message, XmlNode node, string parseText, Type convertToType, Exception innerException = null)
            : base(message ?? Resource1.Parse_InvalidConvert, node, innerException)
        {
            this.ParseText = parseText;
            this.ConvertToType = convertToType;
        }

        public string ParseText { get; private set; }

        public Type ConvertToType { get; private set; }

        public override string Message
        {
            get
            {
                string message = base.Message;

                message += Environment.NewLine + Resource1.Read_ParseText.FormatArgs(ParseText);
                if (ConvertToType != null)
                    message += Environment.NewLine + Resource1.Parse_ConvertTypeName.FormatArgs(ConvertToType.FullName);
                return message;
            }
        }

    }
}
