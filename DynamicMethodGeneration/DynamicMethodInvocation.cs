namespace DynamicMethodGeneration
{
    public class DynamicMethodInvocation<TInstance> : IDynamicMethod
    {
        public DynamicMethod Method { get; internal set; }
        public TInstance Instance { get; internal set; }

        public void Invoke()
        {
            Method.Invoke(Instance);
        }

        public void Invoke(object instance, params object[] args)
        {
            // TODO: We just ignore the member variable??
            Method.Invoke(instance, args);
        }

        public void Invoke(params object[] args)
        {
            Method.Invoke(Instance, args);
        }

        public void Invoke<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            Method.Invoke(Instance, arg1, arg2, arg3);
        }

        public void Invoke<TArg1, TArg2>(TArg1 arg1, TArg2 arg2)
        {
            Method.Invoke(Instance, arg1, arg2);
        }

        public void Invoke<TArg1>(TArg1 arg1)
        {
            Method.Invoke(Instance, arg1);
        }
    }

    public class DynamicMethodInvocation<TInstance, TReturn> : IDynamicMethod<TReturn>
    {
        public DynamicMethod<TReturn> Method { get; internal set; }
        public TInstance Instance { get; internal set; }

        public TReturn Invoke()
        {
            return Method.Invoke(Instance);
        }

        public TReturn Invoke(object instance, params object[] args)
        {
            // TODO: We just ignore the member variable??
            return Method.Invoke(instance, args);
        }

        public TReturn Invoke(params object[] args)
        {
            return Method.Invoke(Instance, args);
        }

        public TReturn Invoke<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            return Method.Invoke(Instance, arg1, arg2, arg3);
        }

        public TReturn Invoke<TArg1, TArg2>(TArg1 arg1, TArg2 arg2)
        {
            return Method.Invoke(Instance, arg1, arg2);
        }

        public TReturn Invoke<TArg1>(TArg1 arg1)
        {
            return Method.Invoke(Instance, arg1);
        }
    }
}
