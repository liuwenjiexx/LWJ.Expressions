
using System;

namespace LWJ.Expressions
{

    internal class VariableInfo
    {
        public Type Type;
        public string Name;
        public object Value;
        public object DefaultValue;

        public VariableInfo(Type type, string name, object defaultValue)
        {
            this.Type = type;
            this.Name = name;
            this.Value = defaultValue;
            this.DefaultValue = defaultValue;
        }
    }
}
