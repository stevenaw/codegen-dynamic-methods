namespace DynamicMethodGeneration
{
    public interface IMethod
    {

    }

    public interface IDynamicMethod : IMethod
    {
        void Invoke();
        void Invoke(params object[] args);
        void Invoke<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3);
        void Invoke<TArg1, TArg2>(TArg1 arg1, TArg2 arg2);
        void Invoke<TArg1>(TArg1 arg1);
    }

    public interface IDynamicMethod<TReturn> : IMethod
    {
        TReturn InvokeAndReturn();
        TReturn InvokeAndReturn(params object[] args);
        TReturn InvokeAndReturn<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3);
        TReturn InvokeAndReturn<TArg1, TArg2>(TArg1 arg1, TArg2 arg2);
        TReturn InvokeAndReturn<TArg1>(TArg1 arg1);
    }
}