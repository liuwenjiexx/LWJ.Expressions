/**************************************************************
 *  Filename:    DefaultOperators.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/

namespace LWJ.Expressions
{
    /// <summary>
    /// internal ios 不编译未直接调用的方法
    /// </summary>
    public partial class DefaultOperators
    {
        #region Add

        [OverrideOperator]
        public static sbyte Add(sbyte a, sbyte b) => (sbyte)(a + b);
        [OverrideOperator]
        public static short Add(sbyte a, short b) => (short)(a + b);
        [OverrideOperator]
        public static int Add(sbyte a, int b) => a + b;
        [OverrideOperator]
        public static long Add(sbyte a, long b) => a + b;
        [OverrideOperator]
        public static float Add(sbyte a, float b) => a + b;
        [OverrideOperator]
        public static double Add(sbyte a, double b) => a + b;

        [OverrideOperator]
        public static short Add(short a, short b) => (short)(a + b);
        [OverrideOperator]
        public static int Add(short a, int b) => a + b;
        [OverrideOperator]
        public static long Add(short a, long b) => a + b;
        [OverrideOperator]
        public static float Add(short a, float b) => a + b;
        [OverrideOperator]
        public static double Add(short a, double b) => a + b;

        [OverrideOperator]
        public static int Add(int a, sbyte b) => a + b;
        [OverrideOperator]
        public static int Add(int a, short b) => a + b;
        [OverrideOperator]
        public static int Add(int a, int b) => a + b;
        [OverrideOperator]
        public static long Add(int a, long b) => a + b;
        [OverrideOperator]
        public static float Add(int a, float b) => a + b;
        [OverrideOperator]
        public static double Add(int a, double b) => a + b;

        [OverrideOperator]
        public static long Add(long a, sbyte b) => a + b;
        [OverrideOperator]
        public static long Add(long a, short b) => a + b;
        [OverrideOperator]
        public static long Add(long a, int b) => a + b;
        [OverrideOperator]
        public static double Add(long a, long b) => a + b;
        [OverrideOperator]
        public static float Add(long a, float b) => a + b;
        [OverrideOperator]
        public static double Add(long a, double b) => a + b;

        [OverrideOperator]
        public static float Add(float a, sbyte b) => a + b;
        [OverrideOperator]
        public static float Add(float a, short b) => a + b;
        [OverrideOperator]
        public static float Add(float a, int b) => a + b;
        [OverrideOperator]
        public static float Add(float a, long b) => a + b;
        [OverrideOperator]
        public static float Add(float a, float b) => a + b;
        [OverrideOperator]
        public static double Add(float a, double b) => a + b;

        [OverrideOperator]
        public static double Add(double a, sbyte b) => a + b;
        [OverrideOperator]
        public static double Add(double a, short b) => a + b;
        [OverrideOperator]
        public static double Add(double a, int b) => a + b;
        [OverrideOperator]
        public static double Add(double a, long b) => a + b;
        [OverrideOperator]
        public static double Add(double a, float b) => a + b;
        [OverrideOperator]
        public static double Add(double a, double b) => a + b;

        [OverrideOperator]
        public static string Add(string a, string b) => a + b;

        #endregion



        #region Subtract

        [OverrideOperator]
        public static int Subtract(int a, sbyte b) => a - b;
        [OverrideOperator]
        public static int Subtract(int a, short b) => a - b;
        [OverrideOperator]
        public static int Subtract(int a, int b) => a - b;
        [OverrideOperator]
        public static long Subtract(int a, long b) => a - b;
        [OverrideOperator]
        public static float Subtract(int a, float b) => a - b;
        [OverrideOperator]
        public static double Subtract(int a, double b) => a - b;

        [OverrideOperator]
        public static long Subtract(long a, sbyte b) => a - b;
        [OverrideOperator]
        public static long Subtract(long a, short b) => a - b;
        [OverrideOperator]
        public static long Subtract(long a, int b) => a - b;
        [OverrideOperator]
        public static float Subtract(long a, long b) => a - b;
        [OverrideOperator]
        public static float Subtract(long a, float b) => a - b;
        [OverrideOperator]
        public static double Subtract(long a, double b) => a - b;

        [OverrideOperator]
        public static float Subtract(float a, sbyte b) => a - b;
        [OverrideOperator]
        public static float Subtract(float a, short b) => a - b;
        [OverrideOperator]
        public static float Subtract(float a, int b) => a - b;
        [OverrideOperator]
        public static float Subtract(float a, long b) => a - b;
        [OverrideOperator]
        public static float Subtract(float a, float b) => a - b;
        [OverrideOperator]
        public static double Subtract(float a, double b) => a - b;

        [OverrideOperator]
        public static double Subtract(double a, sbyte b) => a - b;
        [OverrideOperator]
        public static double Subtract(double a, short b) => a - b;
        [OverrideOperator]
        public static double Subtract(double a, int b) => a - b;
        [OverrideOperator]
        public static double Subtract(double a, long b) => a - b;
        [OverrideOperator]
        public static double Subtract(double a, float b) => a - b;
        [OverrideOperator]
        public static double Subtract(double a, double b) => a - b;


        #endregion


        #region Multiply


        [OverrideOperator]
        public static int Multiply(int a, int b) => a * b;
        [OverrideOperator]
        public static long Multiply(int a, long b) => a * b;
        [OverrideOperator]
        public static float Multiply(int a, float b) => a * b;
        [OverrideOperator]
        public static double Multiply(int a, double b) => a * b;
        [OverrideOperator]
        public static long Multiply(long a, int b) => a * b;
        [OverrideOperator]
        public static long Multiply(long a, long b) => a * b;
        [OverrideOperator]
        public static float Multiply(long a, float b) => a * b;
        [OverrideOperator]
        public static double Multiply(long a, double b) => a * b;
        [OverrideOperator]
        public static float Multiply(float a, int b) => a * b;
        [OverrideOperator]
        public static float Multiply(float a, long b) => a * b;
        [OverrideOperator]
        public static float Multiply(float a, float b) => a * b;
        [OverrideOperator]
        public static double Multiply(float a, double b) => a * b;
        [OverrideOperator]
        public static double Multiply(double a, int b) => a * b;
        [OverrideOperator]
        public static double Multiply(double a, long b) => a * b;
        [OverrideOperator]
        public static double Multiply(double a, float b) => a * b;
        [OverrideOperator]
        public static double Multiply(double a, double b) => a * b;

        #endregion

        #region Divide


        [OverrideOperator]
        public static int Divide(int a, int b) => a / b;
        [OverrideOperator]
        public static long Divide(int a, long b) => a / b;
        [OverrideOperator]
        public static float Divide(int a, float b) => a / b;
        [OverrideOperator]
        public static double Divide(int a, double b) => a / b;
        [OverrideOperator]
        public static double Divide(long a, int b) => a / b;
        [OverrideOperator]
        public static double Divide(long a, long b) => a / b;
        [OverrideOperator]
        public static float Divide(long a, float b) => a / b;
        [OverrideOperator]
        public static double Divide(long a, double b) => a / b;
        [OverrideOperator]
        public static float Divide(float a, int b) => a / b;
        [OverrideOperator]
        public static float Divide(float a, long b) => a / b;
        [OverrideOperator]
        public static float Divide(float a, float b) => a / b;
        [OverrideOperator]
        public static double Divide(float a, double b) => a / b;
        [OverrideOperator]
        public static double Divide(double a, int b) => a / b;
        [OverrideOperator]
        public static double Divide(double a, long b) => a / b;
        [OverrideOperator]
        public static double Divide(double a, float b) => a / b;
        [OverrideOperator]
        public static double Divide(double a, double b) => a / b;

        #endregion

        #region Modulo


        [OverrideOperator]
        public static int Modulo(int a, int b) => a % b;
        [OverrideOperator]
        public static long Modulo(int a, long b) => a % b;
        [OverrideOperator]
        public static float Modulo(int a, float b) => a % b;
        [OverrideOperator]
        public static double Modulo(int a, double b) => a % b;
        [OverrideOperator]
        public static double Modulo(long a, int b) => a % b;
        [OverrideOperator]
        public static double Modulo(long a, long b) => a % b;
        [OverrideOperator]
        public static float Modulo(long a, float b) => a % b;
        [OverrideOperator]
        public static double Modulo(long a, double b) => a % b;
        [OverrideOperator]
        public static float Modulo(float a, int b) => a % b;
        [OverrideOperator]
        public static float Modulo(float a, long b) => a % b;
        [OverrideOperator]
        public static float Modulo(float a, float b) => a % b;
        [OverrideOperator]
        public static double Modulo(float a, double b) => a % b;
        [OverrideOperator]
        public static double Modulo(double a, int b) => a % b;
        [OverrideOperator]
        public static double Modulo(double a, long b) => a % b;
        [OverrideOperator]
        public static double Modulo(double a, float b) => a % b;
        [OverrideOperator]
        public static double Modulo(double a, double b) => a % b;

        #endregion

        #region Negate

        [OverrideOperator]
        public static int Negate(int a) => -a;
        [OverrideOperator]
        public static long Negate(long a) => -a;
        [OverrideOperator]
        public static float Negate(float a) => -a;
        [OverrideOperator]
        public static double Negate(double a) => -a;

        #endregion




    }
}
