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

        // TODO: A single call to Compile<TResult>(PropertyInfo), with Invoke() that sets and Invoke<TResult>() that gets
        public static DynamicMethod CompileSetter(this PropertyInfo methodInfo)
        {
            return GetAction(methodInfo.SetMethod);
        }
        public static DynamicMethod<TResult> CompileGetter<TResult>(this PropertyInfo methodInfo)
        {
            return GetFunction<TResult>(methodInfo.GetMethod);
        }

        // TODO: Clean this part up, consolidate with method-based stuff
        public static DynamicMethod CompileSetter(this FieldInfo methodInfo)
        {
            var method = _cache.Get(methodInfo) as DynamicMethod;
            if (method == null)
            {
                var request = DynamicMethodRequest.MakeSetterRequest(methodInfo);
                method = _factory.GetAction(request);
                _cache.Add(methodInfo, method);
            }

            return method;
        }

        public static DynamicMethod<TResult> CompileGetter<TResult>(this FieldInfo methodInfo)
        {
            var method = _cache.Get(methodInfo) as DynamicMethod<TResult>;
            if (method == null)
            {
                var request = DynamicMethodRequest.MakeGetterRequest(methodInfo);
                method = _factory.GetFunction<TResult>(request);
                _cache.Add(methodInfo, method);
            }

            return method;
        }


        private static DynamicMethod GetAction(MethodInfo methodInfo)
        {
            var method = _cache.Get(methodInfo) as DynamicMethod;
            if (method == null)
            {
                var request = DynamicMethodRequest.MakeRequest(methodInfo);
                method = _factory.GetAction(request);
                _cache.Add(methodInfo, method);
            }

            return method;
        }

        private static DynamicMethod<TResult> GetFunction<TResult>(MethodInfo methodInfo)
        {
            var method = _cache.Get(methodInfo) as DynamicMethod<TResult>;
            if (method == null)
            {
                var request = DynamicMethodRequest.MakeRequest(methodInfo);
                method = _factory.GetFunction<TResult>(request);
                _cache.Add(methodInfo, method);
            }

            return method;
        }
    }
}
