using System;
using System.Reflection;
using System.Reflection.Emit;
using Emit = System.Reflection.Emit;

namespace DynamicMethodGeneration
{
    // TODO: Test support for fields, properties, indexers etc
    internal class DynamicMethodFactory
    {
        public DynamicMethod GetAction(DynamicMethodRequest methodRequest)
        {
            var method = GetDelegate(methodRequest);
            return new DynamicMethod
            {
                Invoker = method.invoker,
                UnderlyingType = method.delegateType
            };
        }

        public DynamicMethod<TResult> GetFunction<TResult>(DynamicMethodRequest methodRequest)
        {
            var method = GetDelegate(methodRequest);
            return new DynamicMethod<TResult>
            {
                Invoker = method.invoker,
                UnderlyingType = method.delegateType
            };
        }

        internal (Delegate invoker, Type delegateType) GetDelegate(DynamicMethodRequest methodRequest)
        {
            var argTypes = GetArgTypes(methodRequest);
            var method = new Emit.DynamicMethod(methodRequest.Member.Name, methodRequest.ReturnType, argTypes);
            CreateMethodBody(method, methodRequest, argTypes);

            var delegateType = CreateDelegateType(argTypes, methodRequest.ReturnType);
            var invoker = method.CreateDelegate(delegateType);

            return (invoker, delegateType);
        }

        internal static Type[] GetArgTypes(DynamicMethodRequest methodRequest)
        {
            var parameterInfo = methodRequest.Parameters;
            Type[] argTypes;

            if (methodRequest.IsStatic)
            {
                argTypes = new Type[parameterInfo.Length];
                for (var i = 0; i < parameterInfo.Length; i++)
                    argTypes[i] = parameterInfo[i].ParameterType;
            }
            else
            {
                argTypes = new Type[parameterInfo.Length + 1];
                argTypes[0] = methodRequest.Member.DeclaringType;
                for (var i = 0; i < parameterInfo.Length; i++)
                    argTypes[i + 1] = parameterInfo[i].ParameterType;
            }

            return argTypes;
        }

        internal static void CreateMethodBody(Emit.DynamicMethod dynamicMethod, DynamicMethodRequest methodRequest, Type[] args)
        {
            var generator = dynamicMethod.GetILGenerator();

            for (var i = 0; i < args.Length; i++)
            {
                generator.Emit(OpCodes.Ldarg, i);
            }

            if(methodRequest.Member is MethodInfo)
            {
                var method = (MethodInfo)methodRequest.Member;
                var opCode = methodRequest.IsStatic ? OpCodes.Call : OpCodes.Callvirt;

                generator.EmitCall(opCode, method, null);
            }

            generator.Emit(OpCodes.Ret);
        }

        internal static Type CreateDelegateType(Type[] genericArgs, Type returnType)
        {
            var hasReturnType = returnType != null && returnType != typeof(void);

            // TODO: Rest of these
            Type baseType = null;
            switch (genericArgs.Length)
            {
                case 0:
                    baseType = hasReturnType ? typeof(Func<>) : typeof(Action);
                    break;
                case 1:
                    baseType = hasReturnType ? typeof(Func<,>) : typeof(Action<>);
                    break;
                case 2:
                    baseType = hasReturnType ? typeof(Func<,,>) : typeof(Action<,>);
                    break;
                case 3:
                    baseType = hasReturnType ? typeof(Func<,,,>) : typeof(Action<,,>);
                    break;
                case 4:
                    baseType = hasReturnType ? typeof(Func<,,,,>) : typeof(Action<,,,>);
                    break;
                case 5:
                    baseType = hasReturnType ? typeof(Func<,,,,,>) : typeof(Action<,,,,>);
                    break;
                case 6:
                    baseType = hasReturnType ? typeof(Func<,,,,,,>) : typeof(Action<,,,,,>);
                    break;
                case 7:
                    baseType = hasReturnType ? typeof(Func<,,,,,,,>) : typeof(Action<,,,,,,>);
                    break;
            }

            var args = hasReturnType ? ArrayHelper.Append(genericArgs, returnType) : genericArgs;
            if (args.Length == 0)
                return baseType;

            return baseType.MakeGenericType(args);
        }
    }
}
