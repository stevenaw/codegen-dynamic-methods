using System;
using System.Reflection;
using System.Reflection.Emit;

namespace DynamicMethodGeneration
{
    // TODO: Test support for fields, properties, etc
    // TODO: Verify that methodinfo found matches the args + types provided
    internal class DynamicMethodFactory
    {
        public Delegate GetAction(Type type, string methodName, params object[] args)
        {
            return GetAction(type, methodName, null, args);
        }

        public Delegate GetAction(object instance, string methodName, params object[] args)
        {
            return GetAction(instance.GetType(), methodName, instance, args);
        }

        public Delegate GetFunction<TResult>(Type type, string methodName, params object[] args)
        {
            return GetFunction<TResult>(type, methodName, null, args);
        }

        public Delegate GetFunction<TResult>(object instance, string methodName, params object[] args)
        {
            return GetFunction<TResult>(instance.GetType(), methodName, instance, args);
        }


        private Delegate GetAction(Type type, string methodName, object instance, params object[] args)
        {
            return GetDelegate(type, methodName, typeof(void), instance, args);
        }

        private Delegate GetFunction<TResult>(Type type, string methodName, object instance, object[] args)
        {
            return GetDelegate(type, methodName, typeof(TResult), instance, args);
        }

        private Delegate GetDelegate(Type type, string methodName, Type returnType, object instance, object[] args)
        {
            if (args == null)
                args = new object[0];

            var argTypes = Type.GetTypeArray(args);
            var desiredFunction = type.GetMethod(methodName);

            // TODO: Check if method is static before adding this
            if (instance != null)
            {
                argTypes = ArrayHelper.Prepend(argTypes, type);
            }

            var method = new DynamicMethod(methodName, desiredFunction.ReturnType, argTypes);
            CreateMethodBody(method, desiredFunction, argTypes);

            var delegateType = CreateDelegateType(argTypes, returnType);
            var invoker = method.CreateDelegate(delegateType);

            return invoker;
        }

        private static void CreateMethodBody(DynamicMethod dynamicMethod, MethodInfo method, Type[] args)
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
