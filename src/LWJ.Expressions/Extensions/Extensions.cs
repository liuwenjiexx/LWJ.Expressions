/**************************************************************
 *  Filename:    Extensions.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/
using System.Collections.Generic;

namespace LWJ.Expressions
{

    /// <summary>
    /// public extensions
    /// </summary>
    public static class Extensions
    {
         
        public static IEqualityComparer<ExpressionType> EqualityComparer(this ExpressionType source)
            => ExpressionTypeEqualityComparer.Instance;


        /// <summary>
        /// less <see cref="System.GC"/>
        /// </summary>

        internal class ExpressionTypeEqualityComparer : IEqualityComparer<ExpressionType>
        {
            public bool Equals(ExpressionType x, ExpressionType y) => x == y;


            public int GetHashCode(ExpressionType obj) => (int)obj;


            private static ExpressionTypeEqualityComparer instance;

            public static ExpressionTypeEqualityComparer Instance
            {
                get
                {
                    if (instance == null)
                        instance = new ExpressionTypeEqualityComparer();
                    return instance;
                }
            }
        }
    }

}
