namespace DynamicMethodGeneration
{
    public interface IDynamicMethod
    {
        void Invoke();
        void Invoke(object instance, params object[] args);
        void Invoke(params object[] args);
        void Invoke<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3);
        void Invoke<TArg1, TArg2>(TArg1 arg1, TArg2 arg2);
        void Invoke<TArg1>(TArg1 arg1);
    }

    public interface IDynamicMethod<T>
    {
        T Invoke();
        T Invoke(object instance, params object[] args);
        T Invoke(params object[] args);
        T Invoke<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3);
        T Invoke<TArg1, TArg2>(TArg1 arg1, TArg2 arg2);
        T Invoke<TArg1>(TArg1 arg1);
    }
}