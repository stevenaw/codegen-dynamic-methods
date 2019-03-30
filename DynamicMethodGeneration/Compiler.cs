using System.Reflection;

namespace DynamicMethodGeneration
{
    public static class Compiler
    {
        private static DynamicMethodFactory _factory = new DynamicMethodFactory();
        private static DynamicMethodCache _cache = new DynamicMethodCache();
        
        public static DynamicMethod Compile(this MethodInfo methodInfo)
        {
            return  GetAction(methodInfo);
        }

        public static DynamicMethod<TResult> Compile<TResult>(this MethodInfo methodInfo)
        {
            return GetFunction<TResult>(methodInfo);
        }

        private static DynamicMethod GetAction(MethodInfo methodInfo)
        {
            var method = _cache.Get(methodInfo);
            if (method == null)
            {
                method = _factory.GetAction(methodInfo);
                _cache.Add(methodInfo, method);
            }

            return method;
        }

        private static DynamicMethod<TResult> GetFunction<TResult>(MethodInfo methodInfo)
        {
            var method = _cache.Get(methodInfo);
            if (method == null)
            {
                method = _factory.GetFunction<TResult>(methodInfo);
                _cache.Add(methodInfo, method);
            }

            return (DynamicMethod<TResult>)method;
        }
    }
}
