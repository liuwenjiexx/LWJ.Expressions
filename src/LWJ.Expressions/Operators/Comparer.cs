
namespace LWJ.Expressions
{
      partial class DefaultOperators
    {


        #region Equal


        [OverrideOperator]
        public static bool Equal(object a, object b) => object.Equals(a, b);

        #endregion

        #region NotEqual


        [OverrideOperator]
        public static bool NotEqual(object a, object b) => !object.Equals(a, b);

        #endregion

        #region LessThan


        [OverrideOperator]
        public static bool LessThan(int a, int b) => a < b;
        [OverrideOperator]
        public static bool LessThan(int a, long b) => a < b;
        [OverrideOperator]
        public static bool LessThan(int a, float b) => a < b;
        [OverrideOperator]
        public static bool LessThan(int a, double b) => a < b;
        [OverrideOperator]
        public static bool LessThan(long a, int b) => a < b;
        [OverrideOperator]
        public static bool LessThan(long a, long b) => a < b;
        [OverrideOperator]
        public static bool LessThan(long a, float b) => a < b;
        [OverrideOperator]
        public static bool LessThan(long a, double b) => a < b;
        [OverrideOperator]
        public static bool LessThan(float a, int b) => a < b;
        [OverrideOperator]
        public static bool LessThan(float a, long b) => a < b;
        [OverrideOperator]
        public static bool LessThan(float a, float b) => a < b;
        [OverrideOperator]
        public static bool LessThan(float a, double b) => a < b;
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
        public static bool LessThanOrEqual(int a, int b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(int a, long b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(int a, float b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(int a, double b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(long a, int b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(long a, long b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(long a, float b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(long a, double b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(float a, int b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(float a, long b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(float a, float b) => a <= b;
        [OverrideOperator]
        public static bool LessThanOrEqual(float a, double b) => a <= b;
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
        public static bool GreaterThan(int a, int b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(int a, long b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(int a, float b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(int a, double b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(long a, int b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(long a, long b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(long a, float b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(long a, double b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(float a, int b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(float a, long b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(float a, float b) => a > b;
        [OverrideOperator]
        public static bool GreaterThan(float a, double b) => a > b;
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
        public static bool GreaterThanOrEqual(int a, int b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(int a, long b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(int a, float b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(int a, double b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(long a, int b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(long a, long b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(long a, float b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(long a, double b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(float a, int b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(float a, long b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(float a, float b) => a >= b;
        [OverrideOperator]
        public static bool GreaterThanOrEqual(float a, double b) => a >= b;
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
