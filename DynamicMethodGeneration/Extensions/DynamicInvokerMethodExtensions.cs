namespace DynamicMethodGeneration.Extensions
{
    public static class DynamicInvokerMethodExtensions
    {
        private static DynamicMethodInvoker _invoker = new DynamicMethodInvoker();

        public static void InvokeAction(this object target, string methodName, params object[] args)
        {
            _invoker.InvokeAction(target.GetType().GetMethod(methodName), target, args);
        }

        public static TResult InvokeFunction<TResult>(this object target, string methodName, params object[] args)
        {
            return _invoker.InvokeFunction<TResult>(target.GetType().GetMethod(methodName), target, args);
        }
    }
}
