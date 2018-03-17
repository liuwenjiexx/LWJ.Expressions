/**************************************************************
 *  Filename:    MissingAttributeException.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/
using System.Xml;

namespace LWJ.Expressions.Xml
{
    public class MissingAttributeException : AttributeReadException
    {
        public MissingAttributeException(XmlNode node, string attributeName)
               : this(null, node, attributeName)
        {
        }

        public MissingAttributeException(string message, XmlNode node, string attributeName)
            : base(message ?? Resource1.Parse_MissingAttribute, node, attributeName)
        {

        }


    }
}
