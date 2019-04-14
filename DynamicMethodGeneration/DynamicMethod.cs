using System;

namespace DynamicMethodGeneration
{
    // TODO: Verify that delegate matches the args + types provided
    // TODO: Verify TInstance is correct type
    public class DynamicMethod : IDynamicMethod, IInstanceBinder
    {
        internal Delegate Invoker { get; set; }
        internal Type DeclaringType { get; set; }
        internal Type[] ArgumentTypes { get; set; }
        internal bool IsStatic { get; set; }

        public void Invoke()
        {
            ((Action)Invoker)();
        }

        public void Invoke<TArg1>(TArg1 arg1)
        {
            ((Action<TArg1>)Invoker)(arg1);
        }

        public void Invoke<TArg1, TArg2>(TArg1 arg1, TArg2 arg2)
        {
            ((Action<TArg1, TArg2>)Invoker)(arg1, arg2);
        }

        public void Invoke<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            ((Action<TArg1, TArg2, TArg3>)Invoker)(arg1, arg2, arg3);
        }

        public void Invoke(params object[] args)
        {
            Invoker.DynamicInvoke(args);
        }

        public DynamicMethodInvocation<TInstance> WithInstance<TInstance>(TInstance instance)
        {
            if (IsStatic)
                throw new InvalidOperationException("Can not bind instance to a static member");

            return new DynamicMethodInvocation<TInstance>()
            {
                Method = this,
                Instance = instance
            };
        }
    }

    public class DynamicMethod<TReturn> : IDynamicMethod<TReturn>, IInstanceBinder<TReturn>
    {
        internal Delegate Invoker { get; set; }
        internal Type DeclaringType { get; set; }
        internal Type[] ArgumentTypes { get; set; }
        internal bool IsStatic { get; set; }

        public TReturn Invoke()
        {
            return ((Func<TReturn>)Invoker)();
        }

        public TReturn Invoke<TArg1>(TArg1 arg1)
        {
            return ((Func<TArg1, TReturn>)Invoker)(arg1);
        }

        public TReturn Invoke<TArg1, TArg2>(TArg1 arg1, TArg2 arg2)
        {
            return ((Func<TArg1, TArg2, TReturn>)Invoker)(arg1, arg2);
        }

        public TReturn Invoke<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            return ((Func<TArg1, TArg2, TArg3, TReturn>)Invoker)(arg1, arg2, arg3);
        }

        public TReturn Invoke(params object[] args)
        {
            return (TReturn)Invoker.DynamicInvoke(args);
        }

        public DynamicMethodInvocation<TInstance, TReturn> WithInstance<TInstance>(TInstance instance)
        {
            if (IsStatic)
                throw new InvalidOperationException("Can not bind instance to a static member");

            return new DynamicMethodInvocation<TInstance, TReturn>()
            {
                Method = this,
                Instance = instance
            };
        }
    }
}
