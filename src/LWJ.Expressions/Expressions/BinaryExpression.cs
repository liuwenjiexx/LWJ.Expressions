/**************************************************************
 *  Filename:    BinaryExpression.cs
 *  Description: LWJ.Expressions ClassFile
 *  @author:     WenJie Liu
 *  @version     2017/4/2
 **************************************************************/
using System;
using System.Reflection;

namespace LWJ.Expressions
{

    public class BinaryExpression : Expression
    {
        private Expression left;
        private Expression right;
        private MethodInfo method;
        private bool isEvalBefore;

        internal BinaryExpression(ExpressionType expressionType, Expression left, Expression right, MethodInfo method, bool isEvalBefore = false)
            : base(expressionType, method != null ? method.ReturnType : typeof(void))
        {
            this.left = left ?? throw new ArgumentNullException(nameof(left));
            this.right = right ?? throw new ArgumentNullException(nameof(right));
            this.method = method;
            this.isEvalBefore = isEvalBefore;
        }



        public Expression Left => left;

        public Expression Right => right;

        public MethodInfo Method => method;



        public override CompiledDelegate Compile(CompileContext ctx)
        {

            var lhs = left.Compile(ctx);
            var rhs = right.Compile(ctx);
            var method = this.method;
            if (isEvalBefore)
                return (invoke) => method.Invoke(null, new object[] { invoke, lhs, rhs });

            return (invoke) => method.Invoke(null, new object[] { lhs(invoke), rhs(invoke) });

        }

        internal static Delegate CreateBinaryExpressionDelegate(Delegate oper, Expression lhs, Expression rhs, Type returnType)
        {
            Type funcType = typeof(Func<>).MakeGenericType(returnType);
            Type valueWrapType = typeof(BinaryExpressionDelegateWrap<>).MakeGenericType(returnType);
            var target = valueWrapType.GetConstructors()[0].Invoke(new object[] { oper, lhs, rhs });
            Delegate del = Delegate.CreateDelegate(funcType, target, "Execute");

            return del;
        }
        internal static Delegate CreateBinaryDelegate(Delegate oper, Delegate lhs, Delegate rhs, Type returnType)
        {
            Type funcType = typeof(Func<>).MakeGenericType(returnType);
            Type valueWrapType = typeof(BinaryDelegateWrap<,,>).MakeGenericType(lhs.Method.ReturnType, rhs.Method.ReturnType, returnType);
            var target = valueWrapType.GetConstructors()[0].Invoke(new object[] { oper, lhs, rhs });
            Delegate del = Delegate.CreateDelegate(funcType, target, "Execute");

            return del;
        }


        class BinaryDelegateWrap<TOper1, TOper2, TResult>
        {
            Delegate oper;
            Delegate lhs;
            Delegate rhs;
            public BinaryDelegateWrap(Delegate oper, Delegate lhs, Delegate rhs)
            {
                this.oper = oper;
                this.lhs = lhs;
                this.rhs = rhs;
            }

            public TResult Execute()
            {
                var a = lhs.DynamicInvoke();
                var b = rhs.DynamicInvoke();
                return (TResult)oper.DynamicInvoke(a, b);
            }

        }
        class BinaryExpressionDelegateWrap<TResult>
        {
            Delegate oper;
            Expression lhs;
            Expression rhs;
            public BinaryExpressionDelegateWrap(Delegate oper, Expression lhs, Expression rhs)
            {
                this.oper = oper;
                this.lhs = lhs;
                this.rhs = rhs;
            }

            public TResult Execute()
            {
                return (TResult)oper.DynamicInvoke(lhs, rhs);
            }

        }

        //public override object Calculate(ExpressionContext ctx)
        //{
        //    if (operInfo.binaryCalculate != null)
        //    {
        //        var a = Left.Calculate(ctx);
        //        var b = Right.Calculate(ctx);
        //        if (operInfo.inverse)
        //            return operInfo.binaryCalculate(b, a);
        //        return operInfo.binaryCalculate(a, b);
        //    }
        //    else
        //    {
        //        return operInfo.binaryCalculate2(ctx, left, right);
        //    }
        //}

    }


}