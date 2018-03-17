/**************************************************************
 *  Filename:    EqualityComparers.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/
using System;
using System.Collections.Generic;

namespace LWJ.Expressions
{
    /*ios not support
    internal class EqualityComparers
    {
        public static readonly IEqualityComparer<KeyValuePair<Type, Type>> KeyValuePairTypeType = new KeyValuePairEqualityComparer<Type, Type>();

        public class KeyValuePairEqualityComparer<TKey, TValue> : IEqualityComparer<KeyValuePair<TKey, TValue>>
        {
            public bool Equals(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
                 => Equals(x.Key, y.Key) && Equals(x.Value, y.Value);

            public int GetHashCode(KeyValuePair<TKey, TValue> obj)
                => obj.GetHashCode();
        }
    }*/

}
