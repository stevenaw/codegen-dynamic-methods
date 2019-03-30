using System;

namespace DynamicMethodGeneration
{
    // TODO: Verify that delegate matches the args + types provided
    public class DynamicMethod : IDynamicMethod
    {
        internal Delegate Invoker { get; set; }
        internal Type UnderlyingType { get; set; }

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
            return new DynamicMethodInvocation<TInstance>()
            {
                Method = this,
                Instance = instance
            };
        }
    }

    public class DynamicMethod<T> : DynamicMethod, IDynamicMethod<T>
    {
        public new T Invoke()
        {
            return ((Func<T>)Invoker)();
        }

        public new T Invoke<TArg1>(TArg1 arg1)
        {
            return ((Func<TArg1, T>)Invoker)(arg1);
        }

        public new T Invoke<TArg1, TArg2>(TArg1 arg1, TArg2 arg2)
        {
            return ((Func<TArg1, TArg2, T>)Invoker)(arg1, arg2);
        }

        public new T Invoke<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            return ((Func<TArg1, TArg2, TArg3, T>)Invoker)(arg1, arg2, arg3);
        }

        public new T Invoke(params object[] args)
        {
            return (T)Invoker.DynamicInvoke(args);
        }


        public new DynamicMethodInvocation<TInstance, T> WithInstance<TInstance>(TInstance instance)
        {
            return new DynamicMethodInvocation<TInstance, T>()
            {
                Method = this,
                Instance = instance
            };
        }
    }
}
