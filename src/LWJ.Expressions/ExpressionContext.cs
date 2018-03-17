using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWJ.Expressions
{
    public class ExpressionContext : IExpressionContext
    {
        private Dictionary<string, VariableInfo> variables;

        private ExpressionContext parent;
        private IExpressionContext context;


        public ExpressionContext()
        {

        }

        public ExpressionContext(ExpressionContext parent)
        {
            this.parent = parent;
        }
        public ExpressionContext(ExpressionContext parent, IExpressionContext context)
        {
            this.parent = parent;
            this.context = context;
        }

        public object this[string variableName]
        {
            get
            {
                return GetVariable(variableName);
            }
            set
            {
                SetVariable(variableName, value);
            }
        }
        public IExpressionContext Parent { get => parent; }

        #region Variable
        public void AddVariable<T>(string name)
            => AddVariable(typeof(T), name);


        public void AddVariable<T>(string name, T defaultValue)
        {
            AddVariable(typeof(T), name, defaultValue);
        }

        public void AddVariable(Type type, string name)
        {
            object defValue;
            if (type.IsValueType)
            {
                defValue = Activator.CreateInstance(type);
            }
            else
            {
                defValue = null;
            }
            AddVariable(type, name, defValue);
        }
         

        public void AddVariable(Type type, string name, object defaultValue)
        {
            if (variables == null)
                variables = new Dictionary<string, VariableInfo>();
            VariableInfo variable = new VariableInfo(type, name, defaultValue);
            variables[name] = variable;
        }

        public bool ContainsVariable(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            ExpressionContext current = this;
            while (current != null)
            {
                if (current.variables != null && current.variables.ContainsKey(name))
                    return true;
                if (current.context != null && current.context.ContainsVariable(name))
                    return true;
                current = current.parent;
            }
            return false;
        }

        public void SetVariable(string name, object value)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            ExpressionContext current = this;
            VariableInfo variable;

            while (current != null)
            {
                if (current.variables != null && current.variables.TryGetValue(name, out variable))
                {
                    if (value != null)
                    {
                        if (!variable.Type.IsAssignableFrom(value.GetType()))
                            throw new MemberAccessException("Variable Value Type Not Assignable");
                    }
                    variable.Value = value;
                    return;
                }
                if (current.context != null && current.context.ContainsVariable(name))
                {
                    current.context.SetVariable(name, value);
                    return;
                }
                current = current.parent;
            }


            throw new VariableException(Resource1.VariableNotFound, name);
        }

        public Type GetVariableType(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            ExpressionContext current = this;
            VariableInfo variable;

            while (current != null)
            {
                if (current.variables != null && current.variables.TryGetValue(name, out variable))
                    return variable.Type;

                if (current.context != null && current.context.ContainsVariable(name))
                    return current.context.GetVariableType(name);

                current = current.parent;
            }

            throw new VariableException(Resource1.VariableNotFound, name);
        }

        public object GetVariable(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            ExpressionContext current = this;
            VariableInfo variable;

            while (current != null)
            {
                if (current.variables != null && current.variables.TryGetValue(name, out variable))
                    return variable.Value;

                if (current.context != null && current.context.ContainsVariable(name))
                    return current.context.GetVariable(name);

                current = current.parent;
            }
            throw new VariableException(Resource1.VariableNotFound, name);
        }



        //public IEnumerable<string> EnumerateLocalVariables()
        //{
        //    if (variables == null)
        //    {
        //        foreach (var variable in variables)
        //            yield return variable.Key;
        //    }
        //}

        //public IEnumerable<string> EnumerateVariables()
        //{
        //    ExpressionContext current = this;
        //    while (current != null)
        //    {
        //        if (current.variables != null)
        //        {
        //            foreach (var name in current.variables.Keys)
        //                yield return name;
        //        }
        //        if (current.variableProvider != null)
        //        {
        //            foreach (var name in current.variableProvider.EnumerateVariables())
        //                yield return name;
        //        }

        //        current = current.parent;
        //    }

        //}


        #endregion

    }
}
