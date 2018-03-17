/**************************************************************
 *  Filename:    IExpressionContext.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/
using System;

namespace LWJ.Expressions
{
    public interface IExpressionContext
    {

        object this[string name] { get; set; }

        bool ContainsVariable(string name); 

        void SetVariable(string name, object value);

        Type GetVariableType(string name);

        object GetVariable(string name);

    }
}
