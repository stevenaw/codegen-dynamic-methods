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

        public void Invoke(params object[] args)
        {
            Method.Invoke(ArrayHelper.Prepend(args, Instance));
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

        public TReturn InvokeAndReturn()
        {
            return Method.InvokeAndReturn(Instance);
        }

        public TReturn InvokeAndReturn(params object[] args)
        {
            return Method.InvokeAndReturn(ArrayHelper.Prepend(args, Instance));
        }

        public TReturn InvokeAndReturn<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            return Method.InvokeAndReturn(Instance, arg1, arg2, arg3);
        }

        public TReturn InvokeAndReturn<TArg1, TArg2>(TArg1 arg1, TArg2 arg2)
        {
            return Method.InvokeAndReturn(Instance, arg1, arg2);
        }

        public TReturn InvokeAndReturn<TArg1>(TArg1 arg1)
        {
            return Method.InvokeAndReturn(Instance, arg1);
        }
    }
}
