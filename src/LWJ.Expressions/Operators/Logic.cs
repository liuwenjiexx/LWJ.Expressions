
namespace LWJ.Expressions
{

      partial class DefaultOperators
    {
        #region Logic

        public static bool Is(object obj)
        {
            if (obj == null)
                return false;
            if (obj is bool)
                return (bool)obj;
            return true;
        }

        [OverrideOperator]
        public static bool And(InvocationContext invoke, CompiledDelegate oper1, CompiledDelegate oper2)
        {
            return Is(oper1(invoke)) && Is(oper2(invoke));
        }

        [OverrideOperator]
        public static bool Or(InvocationContext invoke, CompiledDelegate oper1, CompiledDelegate oper2)
        {
            return Is(oper1(invoke)) || Is(oper2(invoke));
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
