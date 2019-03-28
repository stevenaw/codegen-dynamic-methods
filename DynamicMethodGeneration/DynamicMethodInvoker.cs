using System.Reflection;

namespace DynamicMethodGeneration
{
    // TODO: Don't use DynamicInvoke. Instead use the actual delegate (ex: Action<TestClass, int, int>)
    public class DynamicMethodInvoker
    {
        private static DynamicMethodFactory _factory = new DynamicMethodFactory();
        private static DynamicMethodCache _cache = new DynamicMethodCache();
        
        public void InvokeAction(MethodInfo methodInfo, params object[] args)
        {
            var method = GetAction(methodInfo, null, args);

            method.Invoker.DynamicInvoke(args);
        }

        public void InvokeAction(MethodInfo methodInfo, object instance, params object[] args)
        {
            var method = GetAction(methodInfo, instance, args);

            if (args.Length == 0)
                method.Invoker.DynamicInvoke(instance);
            else
                method.Invoker.DynamicInvoke(ArrayHelper.Prepend(args, instance));
        }

        public TResult InvokeFunction<TResult>(MethodInfo methodInfo, params object[] args)
        {
            var method = GetFunction<TResult>(methodInfo, null, args);

            return (TResult)method.Invoker.DynamicInvoke(args);
        }

        public TResult InvokeFunction<TResult>(MethodInfo methodInfo, object instance, params object[] args)
        {
            var method = GetFunction<TResult>(methodInfo, instance, args);

            if (args.Length == 0)
                return (TResult)method.Invoker.DynamicInvoke(instance);
            else
                return (TResult)method.Invoker.DynamicInvoke(ArrayHelper.Prepend(args, instance));
        }
        
        private static DynamicMethod GetAction(MethodInfo methodInfo, object instance, object[] args)
        {
            var method = _cache.Get(methodInfo);
            if (method == null)
            {
                method = _factory.GetAction(methodInfo, instance, args);
                _cache.Add(methodInfo, method);
            }

            return method;
        }

        private static DynamicMethod GetFunction<TResult>(MethodInfo methodInfo, object instance, object[] args)
        {
            var method = _cache.Get(methodInfo);
            if (method == null)
            {
                method = _factory.GetFunction<TResult>(methodInfo, instance, args);
                _cache.Add(methodInfo, method);
            }

            return method;
        }
    }
}
