using System;

namespace DynamicMethodGeneration
{
    // TODO: Verify that delegate matches the args + types provided
    // TODO: Add helper functions to make passing the instance more intuitive
    //    Decorator pattern maybe?
    //    public DynamicMethod WithInstance<TInstance>() { return this; }
    public class DynamicMethod
    {
        internal Delegate Invoker { get; set; }
        internal Type UnderlyingType { get; set; }
        
        protected static object[] GetArgs(object[] args, object instance = null)
        {
            if (instance == null)
                return args ?? new object[0];
            else if (args == null)
                return new object[] { instance };
            else
                return ArrayHelper.Prepend(args, instance);
        }

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
            // TODO: Try and infer the fn type based on DynamicMethod.UnderlyingType
            Invoker.DynamicInvoke(GetArgs(args));
        }

        public void Invoke(object instance, params object[] args)
        {
            Invoker.DynamicInvoke(GetArgs(args, instance));
        }
    }

    public class DynamicMethod<T> : DynamicMethod
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
            return (T)Invoker.DynamicInvoke(GetArgs(args));
        }

        public new T Invoke(object instance, params object[] args)
        {
            return (T)Invoker.DynamicInvoke(GetArgs(args, instance));
        }
    }
}
