using System;

namespace DynamicMethodGeneration
{
    // TODO: Don't use DynamicInvoke. Instead use the actual delegate (ex: Action<TestClass, int, int>)
    internal class DynamicMethod
    {
        public Delegate Invoker { get; set; }
        public Type UnderlyingType { get; set; }
        
        protected static object[] GetArgs(object[] args, object instance = null)
        {
            if (instance == null)
                return args ?? new object[0];
            else if (args == null)
                return new object[] { instance };
            else
                return ArrayHelper.Prepend(args, instance);
        }

        public void Invoke(params object[] args)
        {
            Invoker.DynamicInvoke(GetArgs(args));
        }

        public void Invoke(object instance, params object[] args)
        {
            Invoker.DynamicInvoke(GetArgs(args, instance));
        }
    }

    internal class DynamicMethod<T> : DynamicMethod
    {
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
