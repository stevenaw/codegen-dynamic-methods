using System.Reflection;

namespace DynamicMethodGeneration
{
    public class DynamicMethodInvoker
    {
        private static DynamicMethodFactory _factory = new DynamicMethodFactory();
        private static DynamicMethodCache _cache = new DynamicMethodCache();
        
        public void InvokeAction(MethodInfo methodInfo, params object[] args)
        {
            var method = GetAction(methodInfo, null);

            method.Invoke(args);
        }

        public void InvokeAction(MethodInfo methodInfo, object instance, params object[] args)
        {
            var method = GetAction(methodInfo, instance);

            method.Invoke(instance, args);
        }

        public TResult InvokeFunction<TResult>(MethodInfo methodInfo, params object[] args)
        {
            var method = GetFunction<TResult>(methodInfo, null);
            
            return method.Invoke(args);
        }

        public TResult InvokeFunction<TResult>(MethodInfo methodInfo, object instance, params object[] args)
        {
            var method = GetFunction<TResult>(methodInfo, instance);

            return method.Invoke(instance, args);
        }
        
        private static DynamicMethod GetAction(MethodInfo methodInfo, object instance)
        {
            var method = _cache.Get(methodInfo);
            if (method == null)
            {
                method = _factory.GetAction(methodInfo, instance);
                _cache.Add(methodInfo, method);
            }

            return method;
        }

        private static DynamicMethod<TResult> GetFunction<TResult>(MethodInfo methodInfo, object instance)
        {
            var method = _cache.Get(methodInfo);
            if (method == null)
            {
                method = _factory.GetFunction<TResult>(methodInfo, instance);
                _cache.Add(methodInfo, method);
            }

            return (DynamicMethod<TResult>)method;
        }
    }
}
