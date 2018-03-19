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

        [OverrideOperator]
        public static sbyte Convert(object oper1, sbyte oper2) => (sbyte)oper1;

        [OverrideOperator]
        public static ushort Convert(object oper1, ushort oper2) => (ushort)oper1;

        [OverrideOperator]
        public static uint Convert(object oper1, uint oper2) => (uint)oper1;

        [OverrideOperator]
        public static ulong Convert(object oper1, ulong oper2) => (ulong)oper1;


        [OverrideOperator]
        public static byte Convert(object oper1, byte oper2) => (byte)oper1;

        [OverrideOperator]
        public static short Convert(object oper1, short oper2) => (short)oper1;

        #region To Int32

        [OverrideOperator]
        public static int Convert(object oper1, int oper2) => (int)oper1;
        //[OverrideOperator]
        //public static int Convert(byte oper1, int oper2) => (int)oper1;
        //[OverrideOperator]
        //public static int Convert(short oper1, int oper2) => (int)oper1;
        //[OverrideOperator]
        //public static int Convert(long oper1, int oper2) => (int)oper1;
        //[OverrideOperator]
        //public static int Convert(float oper1, int oper2) => (int)oper1;
        //[OverrideOperator]
        //public static int Convert(double oper1, int oper2) => (int)oper1;

        #endregion

        #region To Int64


        [OverrideOperator]
        public static long Convert(object oper1, long oper2) => (long)oper1;
        //[OverrideOperator]
        //public static long Convert(byte oper1, long oper2) => (long)oper1;
        //[OverrideOperator]
        //public static long Convert(short oper1, long oper2) => (long)oper1;
        //[OverrideOperator]
        //public static long Convert(int oper1, long oper2) => (long)oper1;
        //[OverrideOperator]
        //public static long Convert(float oper1, long oper2) => (long)oper1;
        //[OverrideOperator]
        //public static long Convert(double oper1, long oper2) => (long)oper1;


        #endregion


        #region To Float32


        [OverrideOperator]
        public static float Convert(object oper1, float oper2) => (float)oper1;
        //[OverrideOperator]
        //public static float Convert(byte oper1, float oper2) => (float)oper1;
        //[OverrideOperator]
        //public static float Convert(short oper1, float oper2) => (float)oper1;
        //[OverrideOperator]
        //public static float Convert(int oper1, float oper2) => (float)oper1;
        //[OverrideOperator]
        //public static float Convert(long oper1, float oper2) => (float)oper1;

        #endregion

        #region To Float32
            

        [OverrideOperator]
        public static double Convert(object oper1, double oper2) => (double)oper1;
        //[OverrideOperator]
        //public static double Convert(byte oper1, double oper2) => (double)oper1;
        //[OverrideOperator]
        //public static double Convert(short oper1, double oper2) => (double)oper1;
        //[OverrideOperator]
        //public static double Convert(int oper1, double oper2) => (double)oper1;
        //[OverrideOperator]
        //public static double Convert(long oper1, double oper2) => (double)oper1;
        //[OverrideOperator]
        //public static double Convert(float oper1, double oper2) => (double)oper1;

        #endregion


        #endregion

    }
}
