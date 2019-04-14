using System;

namespace DynamicMethodGeneration
{
    public class BidirectionalDynamicMethod<TReturn>
    {
        public IDynamicMethod<TReturn> Get { get; internal set; }
        public IDynamicMethod Set { get; internal set; }

        internal Type DeclaringType { get; set; }
        internal bool IsStatic { get; set; }

        public BidirectionalDynamicMethod<TReturn> WithInstance<TInstance>(TInstance instance)
        {
            Guard.CanBindInstance<TInstance>(DeclaringType, IsStatic);

            var getter = new DynamicMethodInvocation<TInstance, TReturn>()
            {
                Method = Get,
                Instance = instance
            };
            var setter = new DynamicMethodInvocation<TInstance>()
            {
                Method = Set,
                Instance = instance
            };

            return new BidirectionalDynamicMethod<TReturn>()
            {
                Get = getter,
                Set = setter,
                DeclaringType = DeclaringType,
                IsStatic = IsStatic,
            };
        }
    }
}
