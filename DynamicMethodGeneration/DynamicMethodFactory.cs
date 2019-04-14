using System;
using System.Reflection;
using System.Reflection.Emit;
using Emit = System.Reflection.Emit;

namespace DynamicMethodGeneration
{
    // TODO: varargs/params
    internal class DynamicMethodFactory
    {
        public DynamicMethod GetAction(DynamicMethodRequest methodRequest)
        {
            var method = GetDelegate(methodRequest);
            return new DynamicMethod
            {
                Invoker = method.invoker,
                DeclaringType = method.declaringType,
                ArgumentTypes = method.argTypes,
                IsStatic = methodRequest.IsStatic,
            };
        }

        public DynamicMethod<TResult> GetFunction<TResult>(DynamicMethodRequest methodRequest)
        {
            var method = GetDelegate(methodRequest);
            return new DynamicMethod<TResult>
            {
                Invoker = method.invoker,
                DeclaringType = method.declaringType,
                ArgumentTypes = method.argTypes,
                IsStatic = methodRequest.IsStatic,
            };
        }

        internal (Delegate invoker, Type[] argTypes, Type declaringType) GetDelegate(DynamicMethodRequest methodRequest)
        {
            var declaringType = methodRequest.Member.DeclaringType;

            var argTypes = GetArgTypes(methodRequest);
            var method = new Emit.DynamicMethod(methodRequest.Member.Name, methodRequest.ReturnType, argTypes);
            CreateMethodBody(method, methodRequest, argTypes);

            var delegateType = CreateDelegateType(argTypes, methodRequest.ReturnType);
            var invoker = method.CreateDelegate(delegateType);

            return (invoker, argTypes, declaringType);
        }

        internal static Type[] GetArgTypes(DynamicMethodRequest methodRequest)
        {
            if (methodRequest.IsStatic)
            {
                return methodRequest.ParameterTypes;
            }
            else
            {
                var argTypes = new Type[methodRequest.ParameterTypes.Length + 1];

                argTypes[0] = methodRequest.Member.DeclaringType;

                Array.Copy(methodRequest.ParameterTypes, 0, argTypes, 1, methodRequest.ParameterTypes.Length);

                return argTypes;
            }
        }

        internal static void CreateMethodBody(Emit.DynamicMethod dynamicMethod, DynamicMethodRequest methodRequest, Type[] args)
        {
            var generator = dynamicMethod.GetILGenerator();

            for (var i = 0; i < args.Length; i++)
            {
                generator.Emit(OpCodes.Ldarg, i);
            }

            if (methodRequest.Member is MethodInfo)
            {
                var method = (MethodInfo)methodRequest.Member;
                var opCode = methodRequest.IsStatic ? OpCodes.Call : OpCodes.Callvirt;

                generator.EmitCall(opCode, method, null);
            }
            else if (methodRequest.Member is FieldInfo)
            {
                var method = (FieldInfo)methodRequest.Member;
                var isInput = methodRequest.DataFlowDirection == DataFlowDirection.Input;
                var opCode = methodRequest.IsStatic
                    ? (isInput ? OpCodes.Stsfld : OpCodes.Ldsfld)
                    : (isInput ? OpCodes.Stfld : OpCodes.Ldfld);

                generator.Emit(opCode, method);
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
                case 8:
                    baseType = hasReturnType ? typeof(Func<,,,,,,,,>) : typeof(Action<,,,,,,,>);
                    break;
                case 9:
                    baseType = hasReturnType ? typeof(Func<,,,,,,,,,>) : typeof(Action<,,,,,,,,>);
                    break;
            }

            var args = hasReturnType ? ArrayHelper.Append(genericArgs, returnType) : genericArgs;
            if (args.Length == 0)
                return baseType;

            return baseType.MakeGenericType(args);
        }
    }
}
