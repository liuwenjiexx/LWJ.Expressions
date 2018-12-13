namespace LWJ.Expressions
{
    public enum ExpressionType
    {
        None,

        #region arithmetic


        /// <summary>
        /// (a + b)
        /// </summary>
        Add = 1,
        /// <summary>
        /// (a - b)
        /// </summary>
        Subtract = 2,
        /// <summary>
        /// (a * b)
        /// </summary>
        Multiply = 3,
        /// <summary>
        /// (a / b)
        /// </summary>
        Divide = 4,

        /// <summary>
        /// (a % b)
        /// </summary>
        Modulo = 5,
        /// <summary>
        /// (-a)
        /// </summary>
        Negate = 6,
        /// <summary>
        /// (a++)
        /// </summary>
        PreIncrement,
        /// <summary>
        /// (++a)
        /// </summary>
        PostIncrement,
        /// <summary>
        /// (a--)
        /// </summary>
        PreDecrement,
        /// <summary>
        /// (--a)
        /// </summary>
        PostDecrement,



        #endregion


        #region compare

        /// <summary>
        /// (a == b)
        /// </summary>
        Equal = 11,
        /// <summary>
        /// (a != b)
        /// </summary>
        NotEqual = 12,
        /// <summary>
        /// (a &lt; b)
        /// </summary>
        LessThan = 13,
        /// <summary>
        /// (a &lt;= b)
        /// </summary>
        LessThanOrEqual = 14,
        /// <summary>
        /// (a &gt;= b)
        /// </summary>
        GreaterThan = 15,
        /// <summary>
        /// (a &gt;= b)
        /// </summary>
        GreaterThanOrEqual = 16,

        #endregion
        #region logic

        /// <summary>
        /// (a &amp;&amp; b)
        /// </summary>
        And = 17,
        /// <summary>
        /// (a || b)
        /// </summary>
        Or = 18,
        /// <summary>
        /// (!a)
        /// </summary>
        Not = 19,

        #endregion
        #region control

        /// <summary>
        /// (if-else) (a>b ? a : b)
        /// </summary>
        Conditional = 20,

        /// <summary>
        /// (switch-case)
        /// </summary>
        Switch = 21,

        /// <summary>
        /// (while, for)
        /// </summary>
        Loop = 22,

        /// <summary>
        /// (break, continue, goto, return)
        /// </summary>
        Goto = 23,

        #endregion



        /// <summary>
        /// (int, long, float, double, bool ...) value is  <see cref="System.Type.IsPrimitive"/>
        /// </summary>
        Constant = 30,
        /// <summary>
        /// variable
        /// </summary>
        Variable = 31,
        /// <summary>
        /// (( ... ))
        /// </summary>
        //Group,
        /// <summary>
        /// ({ ... })
        /// </summary>
        Block = 32,


        /// <summary>
        /// (object.PropertyOrField)
        /// </summary>
        Member = 33,

        /// <summary>
        /// <see cref="Expression.Function"/>
        /// </summary>
        Function = 34,
        /// <summary>
        /// (object.method( arguments ))
        /// </summary>
        Call = 35,




        /// <summary>
        /// (a = b)
        /// </summary>
        Assign = 40,
        /// <summary>
        /// (a += b)
        /// </summary>
        AddAssign = 41,
        /// <summary>
        /// (a -= b)
        /// </summary>
        SubtractAssign = 42,
        /// <summary>
        /// (a *= b)
        /// </summary>
        MultiplyAssign = 43,
        /// <summary>
        /// (a /= b)
        /// </summary>
        DivideAssign = 44,
        /// <summary>
        /// (a %= b)
        /// </summary>
        ModuloAssign = 45,




        #region Type

        /// <summary>
        /// ((type)object)
        /// </summary>
        Convert = 50,
        /// <summary>
        /// (object as Type), convert fallback return null
        /// </summary>
        TypeAs = 51,
        /// <summary>
        /// (object is Type), return type bool
        /// </summary>
        TypeIs = 52,

        #endregion


        #region exception

        //Throw,
        //TryCatch,
        //TryFinally,
        #endregion
        /// <summary>
        /// (typeof( TypeName ))
        /// </summary>
        //TypeOf,
        /// <summary>
        /// (default( T ))
        /// </summary>
        //Default,
        /// <summary>
        /// (nameof( member ))
        /// </summary>
        //NameOf,
        //Quote,
        //Property,
        //Data,
        //GroupStart,


    }

}