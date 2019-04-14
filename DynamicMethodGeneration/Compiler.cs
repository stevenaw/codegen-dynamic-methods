using System;
using System.Reflection;

namespace DynamicMethodGeneration
{
    public static class Compiler
    {
        private static DynamicMethodFactory _factory = new DynamicMethodFactory();
        private static DynamicMethodCache _cache = new DynamicMethodCache();

        public static DynamicMethod Compile(this MethodInfo member)
        {
            return  GetAction(member, DynamicMethodRequest.MakeRequest);
        }

        public static DynamicMethod<TResult> Compile<TResult>(this MethodInfo member)
        {
            return GetFunction<TResult, MethodInfo>(member, DynamicMethodRequest.MakeRequest);
        }

        // TODO: Tests
        public static BidirectionalDynamicMethod<TResult> Compile<TResult>(this PropertyInfo member)
        {
            var getter = CompileGetter<TResult>(member);
            var setter = CompileSetter(member);

            return new BidirectionalDynamicMethod<TResult>()
            {
                Get = getter,
                Set = setter,
                DeclaringType = getter.DeclaringType,
                IsStatic = getter.IsStatic,
            };
        }

        public static DynamicMethod CompileSetter(this PropertyInfo member)
        {
            return GetAction(member.SetMethod, DynamicMethodRequest.MakeRequest);
        }
        public static DynamicMethod<TResult> CompileGetter<TResult>(this PropertyInfo member)
        {
            return GetFunction<TResult, MethodInfo>(member.GetMethod, DynamicMethodRequest.MakeRequest);
        }

        // TODO: Tests
        public static BidirectionalDynamicMethod<TResult> Compile<TResult>(this FieldInfo member)
        {
            var getter = CompileGetter<TResult>(member);
            var setter = CompileSetter(member);

            return new BidirectionalDynamicMethod<TResult>()
            {
                Get = getter,
                Set = setter,
                DeclaringType = getter.DeclaringType,
                IsStatic = getter.IsStatic,
            };
        }

        public static DynamicMethod CompileSetter(this FieldInfo member)
        {
            return GetAction(member, DynamicMethodRequest.MakeSetterRequest);
        }
        public static DynamicMethod<TResult> CompileGetter<TResult>(this FieldInfo member)
        {
            return GetFunction<TResult, FieldInfo>(member, DynamicMethodRequest.MakeGetterRequest);
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
