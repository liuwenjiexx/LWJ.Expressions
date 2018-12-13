using System;

namespace LWJ.Expressions
{

    partial class DefaultOperators
    {
        #region Logic

        public static bool IsTrue(object obj)
        {
            if (obj == null)
                return false;
            if (obj is bool)
                return (bool)obj;
            var objType = obj.GetType();
            if (objType.IsPrimitive)
            {
                switch (Type.GetTypeCode(objType))
                {
                    case TypeCode.Int32:
                        return (int)obj != 0;
                    case TypeCode.Int64:
                        return (long)obj != 0;
                    case TypeCode.Single:
                        return (float)obj != 0.0f;
                    case TypeCode.Double:
                        return (double)obj != 0.0d;
                }
            }
            else
            {
                if (objType == typeof(string))
                    return ((string)obj).Length != 0;
            }
            return true;
        }

        [OverrideOperator]
        public static object And(InvocationContext invoke, CompiledDelegate oper1, CompiledDelegate oper2)
        {
            object ret = oper1(invoke);
            if (!IsTrue(ret))
                return ret;
            return oper2(invoke);
            //return Is(oper1(invoke)) && Is(oper2(invoke));
        }

        [OverrideOperator]
        public static object Or(InvocationContext invoke, CompiledDelegate oper1, CompiledDelegate oper2)
        {
            object ret = oper1(invoke);
            if (IsTrue(ret))
                return ret;
            return oper2(invoke);
            //return Is(oper1(invoke)) || Is(oper2(invoke));
        }

        [OverrideOperator]
        public static bool Not(object oper1)
        {
            if (oper1 == null)
                return true;
            if (oper1 is bool)
                return !((bool)oper1);
            return false;
        }

        [OverrideOperator]
        public static bool Not(bool oper1)
        {
            return !oper1;
        }

        #endregion

    }
}
