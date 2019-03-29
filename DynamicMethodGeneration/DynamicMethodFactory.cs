using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Emit = System.Reflection.Emit;

namespace DynamicMethodGeneration
{
    // TODO: Test support for fields, properties, etc
    // TODO: Verify that methodinfo found matches the args + types provided
    internal class DynamicMethodFactory
    {
        public DynamicMethod GetAction(MethodInfo memberInfo, object instance)
        {
            var method = GetDelegate(memberInfo, typeof(void), instance);
            return new DynamicMethod
            {
                Invoker = method.invoker,
                UnderlyingType = method.delegateType
            };
        }

        public DynamicMethod<TResult> GetFunction<TResult>(MethodInfo memberInfo, object instance)
        {
            var method = GetDelegate(memberInfo, typeof(TResult), instance);
            return new DynamicMethod<TResult>
            {
                Invoker = method.invoker,
                UnderlyingType = method.delegateType
            };
        }

        private (Delegate invoker, Type delegateType) GetDelegate(MethodInfo methodInfo, Type returnType, object instance)
        {
            // TODO: Save param types to DynamicMethod so can validate invocations
            var parameterInfo = methodInfo.GetParameters();
            Type[] argTypes;

            if (instance == null)
            {
                argTypes = new Type[parameterInfo.Length];
                for (var i = 0; i < parameterInfo.Length; i++)
                    argTypes[i] = parameterInfo[i].ParameterType;
            }
            else
            {
                // TODO: Check if method is static before adding this
                argTypes = new Type[parameterInfo.Length + 1];
                argTypes[0] = instance.GetType();
                for (var i = 0; i < parameterInfo.Length; i++)
                    argTypes[i + 1] = parameterInfo[i].ParameterType;
            }

            // TODO: Make this name dynamic ??
            var method = new Emit.DynamicMethod(methodInfo.Name, methodInfo.ReturnType, argTypes);
            CreateMethodBody(method, methodInfo, argTypes);

            var delegateType = CreateDelegateType(argTypes, returnType);
            var invoker = method.CreateDelegate(delegateType);

            return (invoker, delegateType);
        }

        private static void CreateMethodBody(Emit.DynamicMethod dynamicMethod, MethodInfo method, Type[] args)
        {
            var generator = dynamicMethod.GetILGenerator();

            for (var i = 0; i < args.Length; i++)
            {
                var opCode = args[i].IsValueType ? OpCodes.Ldarg : OpCodes.Ldarga;
                generator.Emit(opCode, i);
            }

            if (method.IsStatic)
            {
                generator.EmitCall(OpCodes.Call, method, null);
            }
            else
            {
                generator.EmitCall(OpCodes.Callvirt, method, null);
            }

            generator.Emit(OpCodes.Ret);
        }

        private static Type CreateDelegateType(Type[] genericArgs, Type returnType)
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
