/**************************************************************
 *  Filename:    Type.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/
using System;

namespace LWJ.Expressions
{

      partial class DefaultOperators
    {
        #region Type

        [OverrideOperator]
        public static object TypeAs(object oper1, Type oper2)
        {
            if (oper1 == null) return null;
            if (oper2.IsAssignableFrom(oper1.GetType()))
                return oper1;
            else
                return null;
        }

        [OverrideOperator]
        public static bool TypeIs(object oper1, Type oper2)
        {
            if (oper1 == null)
                return false;
            return oper2.IsAssignableFrom(oper1.GetType());
        }
        [OverrideOperator]
        public static object Convert(object oper1, Type oper2)
        {
            if (oper1 == null)
                return null;
            Type valueType = oper1.GetType();
            if (!oper2.IsAssignableFrom(valueType))
            {
                oper1 = System.Convert.ChangeType(oper1, oper2);
            }
            //throw new InvalidCastException("Convert Failed. Value Type: {0},Convert Type: {1}".FormatArgs(valueType.FullName, oper2.FullName));
            return oper1;
        }

        #endregion

    }
}
