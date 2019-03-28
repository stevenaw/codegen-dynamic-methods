using System;

namespace DynamicMethodGeneration
{
    internal class DynamicMethod
    {
        public Delegate Invoker { get; set; }
        public Type UnderlyingType { get; set; }
    }
}
