/**************************************************************
 *  Filename:    OverrideOperatorAttribute.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/
using System;

namespace LWJ.Expressions
{

    /// <summary>
    /// usage static methods
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class OverrideOperatorAttribute : Attribute
    {
        public OverrideOperatorAttribute()
        {
            OperatorType = ExpressionType.None;
        }
        public OverrideOperatorAttribute(ExpressionType expressionType)
        {
            this.OperatorType = expressionType;
        }

        /// <summary>
        /// defualt method name to <see cref="LWJ.Expressions.ExpressionType"/>
        /// </summary>
        public ExpressionType OperatorType { get; set; }



    }

}
