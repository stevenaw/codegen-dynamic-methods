using System;
using System.Reflection;

namespace DynamicMethodGeneration
{
    public static class Compiler
    {
        private static DynamicMethodFactory _factory = new DynamicMethodFactory();
        private static DynamicMethodCache _cache = new DynamicMethodCache();

        public static DynamicMethod Compile(this MethodInfo methodInfo)
        {
            return  GetAction(methodInfo, DynamicMethodRequest.MakeRequest);
        }

        public static DynamicMethod<TResult> Compile<TResult>(this MethodInfo methodInfo)
        {
            return GetFunction<TResult, MethodInfo>(methodInfo, DynamicMethodRequest.MakeRequest);
        }

        // TODO: A single call to Compile<TResult>(PropertyInfo), with Invoke() that sets and Invoke<TResult>() that gets
        public static DynamicMethod CompileSetter(this PropertyInfo methodInfo)
        {
            return GetAction(methodInfo.SetMethod, DynamicMethodRequest.MakeRequest);
        }
        public static DynamicMethod<TResult> CompileGetter<TResult>(this PropertyInfo methodInfo)
        {
            return GetFunction<TResult, MethodInfo>(methodInfo.GetMethod, DynamicMethodRequest.MakeRequest);
        }

        public static DynamicMethod CompileSetter(this FieldInfo methodInfo)
        {
            return GetAction(methodInfo, DynamicMethodRequest.MakeSetterRequest);
        }
        public static DynamicMethod<TResult> CompileGetter<TResult>(this FieldInfo methodInfo)
        {
            return GetFunction<TResult, FieldInfo>(methodInfo, DynamicMethodRequest.MakeGetterRequest);
        }

        private static DynamicMethod GetAction<TMemberInfo>(TMemberInfo memberInfo, Func<TMemberInfo, DynamicMethodRequest> makeRequest) where TMemberInfo : MemberInfo
        {
            var method = _cache.Get(memberInfo) as DynamicMethod;
            if (method == null)
            {
                var request = makeRequest(memberInfo);
                method = _factory.GetAction(request);
                _cache.Add(memberInfo, method);
            }

            return method;
        }

        private static DynamicMethod<TResult> GetFunction<TResult, TMemberInfo>(TMemberInfo memberInfo, Func<TMemberInfo, DynamicMethodRequest> makeRequest) where TMemberInfo : MemberInfo
        {
            var method = _cache.Get(memberInfo) as DynamicMethod<TResult>;
            if (method == null)
            {
                var request = makeRequest(memberInfo);
                method = _factory.GetFunction<TResult>(request);
                _cache.Add(memberInfo, method);
            }

            return method;
        }
    }
}
