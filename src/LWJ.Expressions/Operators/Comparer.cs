using System;

namespace LWJ.Expressions
{
    partial class DefaultOperators
    {

        private static readonly double Epsilon = 0.00001d;
        private static bool Approximately(double a, double b) => b - Epsilon <= a && a <= b + Epsilon;


        #region Equal


        [OverrideOperator]
        public static bool Equal(object a, object b) => object.Equals(a, b);

        [OverrideOperator]
        public static bool Equal(byte a, byte b) => a == b;
        [OverrideOperator]
        public static bool Equal(byte a, short b) => a == b;
        [OverrideOperator]
        public static bool Equal(byte a, int b) => a == b;
        [OverrideOperator]
        public static bool Equal(byte a, long b) => a == b;
        [OverrideOperator]
        public static bool Equal(byte a, float b) => Approximately(a, b);
        [OverrideOperator]
        public static bool Equal(byte a, double b) => Approximately(a, b);

        [OverrideOperator]
        public static bool Equal(short a, byte b) => a == b;
        [OverrideOperator]
        public static bool Equal(short a, short b) => a == b;
        [OverrideOperator]
        public static bool Equal(short a, int b) => a == b;
        [OverrideOperator]
        public static bool Equal(short a, long b) => a == b;
        [OverrideOperator]
        public static bool Equal(short a, float b) => Approximately(a, b);
        [OverrideOperator]
        public static bool Equal(short a, double b) => Approximately(a, b);

        [OverrideOperator]
        public static bool Equal(int a, byte b) => a == b;
        [OverrideOperator]
        public static bool Equal(int a, short b) => a == b;
        [OverrideOperator]
        public static bool Equal(int a, int b) => a == b;
        [OverrideOperator]
        public static bool Equal(int a, long b) => a == b;
        [OverrideOperator]
        public static bool Equal(int a, float b) => Approximately(a, b);
        [OverrideOperator]
        public static bool Equal(int a, double b) => Approximately(a, b);

        [OverrideOperator]
        public static bool Equal(long a, byte b) => a == b;
        [OverrideOperator]
        public static bool Equal(long a, short b) => a == b;
        [OverrideOperator]
        public static bool Equal(long a, int b) => a == b;
        [OverrideOperator]
        public static bool Equal(long a, long b) => a == b;
        [OverrideOperator]
        public static bool Equal(long a, float b) => Approximately(a, b);
        [OverrideOperator]
        public static bool Equal(long a, double b) => Approximately(a, b);

        [OverrideOperator]
        public static bool Equal(float a, byte b) => a == b;
        [OverrideOperator]
        public static bool Equal(float a, short b) => a == b;
        [OverrideOperator]
        public static bool Equal(float a, int b) => a == b;
        [OverrideOperator]
        public static bool Equal(float a, long b) => a == b;
        [OverrideOperator]
        public static bool Equal(float a, float b) => Approximately(a, b);
        [OverrideOperator]
        public static bool Equal(float a, double b) => Approximately(a, b);

        [OverrideOperator]
        public static bool Equal(double a, byte b) => a == b;
        [OverrideOperator]
        public static bool Equal(double a, short b) => a == b;
        [OverrideOperator]
        public static bool Equal(double a, int b) => a == b;
        [OverrideOperator]
        public static bool Equal(double a, long b) => a == b;
        [OverrideOperator]
        public static bool Equal(double a, float b) => Approximately(a, b);
        [OverrideOperator]
        public static bool Equal(double a, double b) => Approximately(a, b);

        #endregion

        #region NotEqual


        [OverrideOperator]
        public static bool NotEqual(object a, object b) => !object.Equals(a, b);

        [OverrideOperator]
        public static bool NotEqual(byte a, byte b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(byte a, short b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(byte a, int b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(byte a, long b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(byte a, float b) => !Approximately(a, b);
        [OverrideOperator]
        public static bool NotEqual(byte a, double b) => !Approximately(a, b);

        [OverrideOperator]
        public static bool NotEqual(short a, byte b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(short a, short b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(short a, int b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(short a, long b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(short a, float b) => !Approximately(a, b);
        [OverrideOperator]
        public static bool NotEqual(short a, double b) => !Approximately(a, b);

        [OverrideOperator]
        public static bool NotEqual(int a, byte b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(int a, short b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(int a, int b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(int a, long b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(int a, float b) => !Approximately(a, b);
        [OverrideOperator]
        public static bool NotEqual(int a, double b) => !Approximately(a, b);

        [OverrideOperator]
        public static bool NotEqual(long a, byte b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(long a, short b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(long a, int b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(long a, long b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(long a, float b) => !Approximately(a, b);
        [OverrideOperator]
        public static bool NotEqual(long a, double b) => !Approximately(a, b);

        [OverrideOperator]
        public static bool NotEqual(float a, byte b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(float a, short b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(float a, int b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(float a, long b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(float a, float b) => !Approximately(a, b);
        [OverrideOperator]
        public static bool NotEqual(float a, double b) => !Approximately(a, b);

        [OverrideOperator]
        public static bool NotEqual(double a, byte b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(double a, short b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(double a, int b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(double a, long b) => a != b;
        [OverrideOperator]
        public static bool NotEqual(double a, float b) => !Approximately(a, b);
        [OverrideOperator]
        public static bool NotEqual(double a, double b) => !Approximately(a, b);


        #endregion

        #region LessThan

        [OverrideOperator]
        public static bool LessThan(byte a, byte b) => a < b;
        [OverrideOperator]
        public static bool LessThan(byte a, short b) => a < b;
        [OverrideOperator]
        public static bool LessThan(byte a, int b) => a < b;
        [OverrideOperator]
        public static bool LessThan(byte a, long b) => a < b;
        [OverrideOperator]
        public static bool LessThan(byte a, float b) => a < b;
        [OverrideOperator]
        public static bool LessThan(byte a, double b) => a < b;

        [OverrideOperator]
        public static bool LessThan(short a, byte b) => a < b;
        [OverrideOperator]
        public static bool LessThan(short a, short b) => a < b;
        [OverrideOperator]
        public static bool LessThan(short a, int b) => a < b;
        [OverrideOperator]
        public static bool LessThan(short a, long b) => a < b;
        [OverrideOperator]
        public static bool LessThan(short a, float b) => a < b;
        [OverrideOperator]
        public static bool LessThan(short a, double b) => a < b;

        [OverrideOperator]
        public static bool LessThan(int a, byte b) => a < b;
        [OverrideOperator]
        public static bool LessThan(int a, short b) => a < b;
        [OverrideOperator]
        public static bool LessThan(int a, int b) => a < b;
        [OverrideOperator]
        public static bool LessThan(int a, long b) => a < b;
        [OverrideOperator]
        public static bool LessThan(int a, float b) => a < b;
        [OverrideOperator]
        public static bool LessThan(int a, double b) => a < b;

        [OverrideOperator]
        public static bool LessThan(long a, byte b) => a < b;
        [OverrideOperator]
        public static bool LessThan(long a, short b) => a < b;
        [OverrideOperator]
        public static bool LessThan(long a, int b) => a < b;
        [OverrideOperator]
        public static bool LessThan(long a, long b) => a < b;
        [OverrideOperator]
        public static bool LessThan(long a, float b) => a < b;
        [OverrideOperator]
        public static bool LessThan(long a, double b) => a < b;

        [OverrideOperator]
        public static bool LessThan(float a, byte b) => a < b;
        [OverrideOperator]
        public static bool LessThan(float a, short b) => a < b;
        [OverrideOperator]
        public static bool LessThan(float a, int b) => a < b;
        [OverrideOperator]
        public static bool LessThan(float a, long b) => a < b;
        [OverrideOperator]
        public static bool LessThan(float a, float b) => a < b;
        [OverrideOperator]
        public static bool LessThan(float a, double b) => a < b;

        [OverrideOperator]
        public static bool LessThan(double a, byte b) => a < b;
        [OverrideOperator]
        public static bool LessThan(double a, short b) => a < b;
        [OverrideOperator]
        public static bool LessThan(double a, int b) => a < b;
        [OverrideOperator]
        public static bool LessThan(double a, long b) => a < b;
        [OverrideOperator]
        public static bool LessThan(double a, float b) => a < b;
        [OverrideOperator]
        public static bool LessThan(double a, double b) => a < b;

        #endregion


        #region LessThanOrEqual

        [OverrideOperator]
        public static bool LessThanOrEqual(byte a, byte b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(byte a, short b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(byte a, int b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(byte a, long b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(byte a, float b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(byte a, double b) => a <= b;

        [OverrideOperator]
        public static bool LessThanOrEqual(short a, byte b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(short a, short b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(short a, int b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(short a, long b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(short a, float b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(short a, double b) => a <= b;

        [OverrideOperator]
        public static bool LessThanOrEqual(int a, byte b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(int a, short b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(int a, int b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(int a, long b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(int a, float b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(int a, double b) => a <= b;

        [OverrideOperator]
        public static bool LessThanOrEqual(long a, byte b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(long a, short b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(long a, int b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(long a, long b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(long a, float b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(long a, double b) => a <= b;

        [OverrideOperator]
        public static bool LessThanOrEqual(float a, byte b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(float a, short b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(float a, int b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(float a, long b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(float a, float b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(float a, double b) => a <= b;

        [OverrideOperator]
        public static bool LessThanOrEqual(double a, byte b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(double a, short b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(double a, int b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(double a, long b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(double a, float b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(double a, double b) => a <= b;

        #endregion


        #region GreaterThan

        [OverrideOperator]
        public static bool GreaterThan(byte a, byte b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(byte a, short b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(byte a, int b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(byte a, long b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(byte a, float b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(byte a, double b) => a > b;

        [OverrideOperator]
        public static bool GreaterThan(short a, byte b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(short a, short b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(short a, int b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(short a, long b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(short a, float b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(short a, double b) => a > b;

        [OverrideOperator]
        public static bool GreaterThan(int a, byte b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(int a, short b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(int a, int b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(int a, long b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(int a, float b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(int a, double b) => a > b;

        [OverrideOperator]
        public static bool GreaterThan(long a, byte b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(long a, short b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(long a, int b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(long a, long b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(long a, float b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(long a, double b) => a > b;

        [OverrideOperator]
        public static bool GreaterThan(float a, byte b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(float a, short b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(float a, int b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(float a, long b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(float a, float b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(float a, double b) => a > b;

        [OverrideOperator]
        public static bool GreaterThan(double a, byte b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(double a, short b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(double a, int b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(double a, long b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(double a, float b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(double a, double b) => a > b;

        #endregion


        #region GreaterThanOrEqual

        [OverrideOperator]
        public static bool GreaterThanOrEqual(byte a, byte b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(byte a, short b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(byte a, int b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(byte a, long b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(byte a, float b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(byte a, double b) => a >= b;

        [OverrideOperator]
        public static bool GreaterThanOrEqual(short a, byte b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(short a, short b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(short a, int b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(short a, long b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(short a, float b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(short a, double b) => a >= b;

        [OverrideOperator]
        public static bool GreaterThanOrEqual(int a, byte b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(int a, short b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(int a, int b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(int a, long b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(int a, float b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(int a, double b) => a >= b;

        [OverrideOperator]
        public static bool GreaterThanOrEqual(long a, byte b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(long a, short b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(long a, int b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(long a, long b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(long a, float b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(long a, double b) => a >= b;

        [OverrideOperator]
        public static bool GreaterThanOrEqual(float a, byte b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(float a, short b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(float a, int b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(float a, long b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(float a, float b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(float a, double b) => a >= b;

        [OverrideOperator]
        public static bool GreaterThanOrEqual(double a, byte b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(double a, short b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(double a, int b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(double a, long b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(double a, float b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(double a, double b) => a >= b;

        #endregion

    }
}
