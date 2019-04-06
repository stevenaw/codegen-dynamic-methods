using System;
using System.Reflection;

namespace DynamicMethodGeneration
{
    internal class DynamicMethodRequest
    {
        public MemberInfo Member { get; set; }
        public Type ReturnType { get; set; }
        public ParameterInfo[] Parameters { get; set; }
        public bool IsStatic { get; set; }

        internal static DynamicMethodRequest MakeRequest(MethodInfo methodInfo)
        {
            return new DynamicMethodRequest()
            {
                Member = methodInfo,
                IsStatic = methodInfo.IsStatic,
                Parameters = methodInfo.GetParameters(),
                ReturnType = methodInfo.ReturnType
            };
        }
    }
}
