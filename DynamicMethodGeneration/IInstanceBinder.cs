namespace DynamicMethodGeneration
{
    public interface IInstanceBinder
    {
        DynamicMethodInvocation<TInstance> WithInstance<TInstance>(TInstance instance);
    }

    public interface IInstanceBinder<TReturn>
    {
        DynamicMethodInvocation<TInstance, TReturn> WithInstance<TInstance>(TInstance instance);
    }
}
