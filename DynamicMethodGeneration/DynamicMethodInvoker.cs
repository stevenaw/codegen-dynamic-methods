using System;

namespace DynamicMethodGeneration
{
    // TODO: Interal cache (tiered? Type -> MethodName + Static/Instance -> ArgListUniqueness
    // TODO: Don't use DynamicInvoke. Instead use the actual delegate (ex: Action<TestClass, int, int>)
    public class DynamicMethodInvoker
    {
        private static DynamicMethodFactory _factory = new DynamicMethodFactory();

        public void InvokeAction(Type type, string methodName, params object[] args)
        {
            var method = _factory.GetAction(type, methodName, args);
            method.DynamicInvoke(args);
        }

        public void InvokeAction(object instance, string methodName, params object[] args)
        {
            var method = _factory.GetAction(instance, methodName, args);

            if (args.Length == 0)
                method.DynamicInvoke(instance);
            else
                method.DynamicInvoke(ArrayHelper.Prepend(args, instance));
        }

        public TResult InvokeFunction<TResult>(Type type, string methodName, params object[] args)
        {
            var method = _factory.GetFunction<TResult>(type, methodName, args);
            return (TResult)method.DynamicInvoke(args);
        }

        public TResult InvokeFunction<TResult>(object instance, string methodName, params object[] args)
        {
            var method = _factory.GetFunction<TResult>(instance, methodName, args);

            if (args.Length == 0)
                return (TResult)method.DynamicInvoke(instance);
            else
                return (TResult)method.DynamicInvoke(ArrayHelper.Prepend(args, instance));
        }
    }
}
