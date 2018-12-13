using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LWJ.Expressions
{
    public class SwitchExpression : Expression
    {

        private Expression testValue;
        private Expression defaultBody;
        private ReadOnlyCollection<SwitchCase> cases;
        internal SwitchExpression(Expression testValue, Expression defaultBody, IEnumerable<SwitchCase> cases)
       : base(ExpressionType.Switch)
        {
            this.testValue = testValue;
            this.defaultBody = defaultBody;
            this.cases = new ReadOnlyCollection<SwitchCase>(cases.ToArray());
        }
        internal SwitchExpression(Expression testValue, Expression defaultBody, params SwitchCase[] cases)
            : this(testValue, defaultBody, (IEnumerable<SwitchCase>)cases)
        {
        }


        public Expression TestValue => testValue;

        public Expression DefaultValue => defaultBody;

        public ReadOnlyCollection<SwitchCase> Cases => cases;

        public override CompiledDelegate Compile(CompileContext ctx)
        {
            var valueEval = testValue.Compile(ctx);
            int caseLength = cases.Count;
            var caseEvals = new Func<InvocationContext, object, bool>[caseLength];
            CompiledDelegate defaultBodyEval = null;

            for (int i = 0; i < caseLength; i++)
            {
                caseEvals[i] = CompileSwitchCase(ctx, cases[i]);
            }

            if (defaultBody != null)
            {
                defaultBodyEval = defaultBody.Compile(ctx);
            }

            return (invoke) =>
            {
                var _caseLength = caseLength;
                var _caseEvals = caseEvals;
                var value = valueEval(invoke);
                for (int i = 0; i < _caseLength; i++)
                {
                    if (_caseEvals[i](invoke, value))
                        return null;
                }
                defaultBodyEval?.Invoke(invoke);
                return null;
            };
        }

        Func<InvocationContext, object, bool> CompileSwitchCase(CompileContext ctx, SwitchCase switchCase)
        {
            int valueLength = switchCase.TestValues.Count;
            var valueEvals = new CompiledDelegate[valueLength];
            for (int i = 0; i < valueLength; i++)
                valueEvals[i] = switchCase.TestValues[i].Compile(ctx);
            var bodyEval = switchCase.Body.Compile(ctx);

            return (invoke, value) =>
            {
                bool isMatch = false;
                for (int i = 0; i < valueLength; i++)
                {
                    if (object.Equals(value, valueEvals[i](invoke)))
                    {
                        isMatch = true;
                        break;
                    }

                }
                if (!isMatch)
                    return false;
                bodyEval(invoke);
                return true;
            };
        }

    }

    public class SwitchCase
    {
        private ReadOnlyCollection<Expression> testValues;
        private Expression body;

        public SwitchCase(Expression testValue, Expression body)
        {
            if (testValue == null)
                throw new ArgumentNullException(nameof(testValue));
            this.testValues = new ReadOnlyCollection<Expression>(new Expression[] { testValue });
            this.body = body ?? throw new ArgumentNullException(nameof(body));
        }
        public SwitchCase(IEnumerable<Expression> testValues, Expression body)
        {
            if (testValues == null) throw new ArgumentNullException(nameof(testValues));
            this.testValues = new ReadOnlyCollection<Expression>(testValues.ToArray());
            if (this.testValues.Count == 0)
                throw new ArgumentException("Empty", nameof(testValues));
            this.body = body ?? throw new ArgumentNullException(nameof(body));
        }

        public ReadOnlyCollection<Expression> TestValues => testValues;

        public Expression Body => body;

    }

}
