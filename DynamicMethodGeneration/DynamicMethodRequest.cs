using System;
using System.Linq;
using System.Reflection;

namespace DynamicMethodGeneration
{
    internal enum DataFlowDirection
    {
        Input,
        Output
    }

    internal class DynamicMethodRequest
    {
        public MemberInfo Member { get; set; }
        public Type ReturnType { get; set; }
        public Type[] ParameterTypes { get; set; }
        public bool IsStatic { get; set; }
        public DataFlowDirection DataFlowDirection { get; set; }

        internal static DynamicMethodRequest MakeRequest(MethodInfo memberInfo)
        {
            return new DynamicMethodRequest()
            {
                Member = memberInfo,
                IsStatic = memberInfo.IsStatic,
                ParameterTypes = memberInfo.GetParameters().Select(o => o.ParameterType).ToArray(),
                ReturnType = memberInfo.ReturnType,
                DataFlowDirection = memberInfo.ReturnType == typeof(void) 
                                    ? DataFlowDirection.Input
                                    : DataFlowDirection.Output
            };
        }

        internal static DynamicMethodRequest MakeGetterRequest(FieldInfo memberInfo)
        {
            return new DynamicMethodRequest()
            {
                Member = memberInfo,
                IsStatic = memberInfo.IsStatic,
                ParameterTypes = new Type[0],
                ReturnType = memberInfo.FieldType,
                DataFlowDirection = DataFlowDirection.Output
            };
        }
        internal static DynamicMethodRequest MakeSetterRequest(FieldInfo memberInfo)
        {
            return new DynamicMethodRequest()
            {
                Member = memberInfo,
                IsStatic = memberInfo.IsStatic,
                ParameterTypes = new Type[] { memberInfo.FieldType },
                ReturnType = typeof(void),
                DataFlowDirection = DataFlowDirection.Input
            };
        }
    }
}
