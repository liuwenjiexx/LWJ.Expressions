using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LWJ.Expressions.Script;


namespace LWJ.Expressions.Script.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CheckNumber()
        {
            CheckExpr("1", 1L);
            CheckExpr("-1", -1L);
            CheckExpr("1.2", 1.2d);
            CheckExpr("0.1", 0.1d);
            CheckExpr(".1", 0.1d);
        }



        [TestMethod]
        public void CheckArithmetic()
        {
            CheckExpr("1+2", 3d);
            CheckExpr("2*3", 6L);
            CheckExpr("4/2", 2d);
            CheckExpr("3%2", 1d);
        }
        [TestMethod]
        public void CheckPriority()
        {
            CheckExpr("1+2*3", 7d);
            CheckExpr("1*2+3", 5D);
        }

        [TestMethod]
        public void CheckLogic()
        {
            CheckExpr("1<2", true);
            CheckExpr("2<1", false);
            CheckExpr("1<=2", true);
            CheckExpr("2<=1", false);
            CheckExpr("2>1", true);
            CheckExpr("1>2", false);
            CheckExpr("2>=1", true);
            CheckExpr("1>=2", false);
            CheckExpr("1==1", true);
            CheckExpr("1==2", false);
            CheckExpr("1!=2", true);
            CheckExpr("1!=1", false);
        }

        [TestMethod]
        public void CheckGroup()
        {
            //  CheckExpr("(1)", 1L);
            // CheckExpr("(-1)", -1L);
            // CheckExpr("(1+2)", 3D);
            //CheckExpr("(1+2)*3", 9D);
            CheckExpr("3*(1+2)", 9D);
        }


        void CheckExpr(string exprString, object expectedResult)
        {
            Expression expr;
            try
            {
                expr = ScriptExpressionReader.Instance.Parse(exprString);
            }
            catch (Exception ex)
            {
                throw new Exception(exprString, ex);
            }
            object result;
            result = expr.Compile(null).Invoke(null);
            Assert.AreEqual(expectedResult, result, exprString);
        }
    }
}
