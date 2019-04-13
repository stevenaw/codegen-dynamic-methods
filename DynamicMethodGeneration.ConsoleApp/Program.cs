using System;

namespace DynamicMethodGeneration.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TestStaticFunctions();
            TestInstanceFunctions();
            TestInstanceProperties();
            TestInstanceFields();

#if !(DEBUG)
            Console.Write("Please press a key...");
            Console.Read();
#endif
        }

        private static void TestInstanceProperties()
        {
            const int testValue = 42;
            var target = new TestInstanceClass();
            var owningType = typeof(TestInstanceClass);
            var propInfo = owningType.GetProperty(nameof(TestInstanceClass.PropertyWithoutArgument));


            var setter = propInfo.CompileSetter();
            var getter = propInfo.CompileGetter<int>();

            setter.WithInstance(target).Invoke(testValue);
            var value = getter.WithInstance(target).InvokeAndReturn();

            Console.WriteLine($"Return value of prop = {value}");
        }

        private static void TestInstanceFields()
        {
            const int testValue = 42;
            var target = new TestInstanceClass();
            var owningType = typeof(TestInstanceClass);
            var propInfo = owningType.GetField(nameof(TestInstanceClass.FieldTest));


            var setter = propInfo.CompileSetter();
            var getter = propInfo.CompileGetter<int>();

            setter.WithInstance(target).Invoke(testValue);
            var value = getter.WithInstance(target).InvokeAndReturn();

            Console.WriteLine($"Return value of field = {value}");
        }

        private static void TestInstanceFunctions()
        {
            var target = new TestInstanceClass();
            var owningType = typeof(TestInstanceClass);

            // Instance method /w no args and no return
            var methodNoArgsNoReturn = owningType.GetMethod(nameof(TestInstanceClass.MethodNoArgsNoReturn)).Compile();
            methodNoArgsNoReturn.WithInstance(target).Invoke();

            // Instance method /w no args and return value
            var methodNoArgsHasReturn = owningType.GetMethod(nameof(TestInstanceClass.MethodNoArgsHasReturn)).Compile<int>();
            var value = methodNoArgsHasReturn.WithInstance(target).InvokeAndReturn();
            Console.WriteLine($"Return value = {value}");

            // Instance method /w args and no return
            var methodHasArgsNoReturn = owningType.GetMethod(nameof(TestInstanceClass.MethodWithArgsAndNoReturn)).Compile();
            methodHasArgsNoReturn.WithInstance(target).Invoke(2, 5);

            // Instance method /w args and return value
            var methodHasArgsHasReturn = owningType.GetMethod(nameof(TestInstanceClass.MethodWithArgsAndReturn)).Compile<int>();
            value = methodHasArgsHasReturn.WithInstance(target).InvokeAndReturn(2, 5);
            Console.WriteLine($"Return value = {value}");

            // Instance method /w args and return value as DynamicInvoke
            var methodManyArgsHasReturn = owningType.GetMethod(nameof(TestInstanceClass.MethodWithManyArgsAndReturn)).Compile<int>();
            value = methodManyArgsHasReturn.WithInstance(target).InvokeAndReturn(2, 3, 5, 8, 13, 21);
            Console.WriteLine($"Return value = {value}");
        }

        private static void TestStaticFunctions()
        {
            var owningType = typeof(TestClass);

            // Static method /w no args and no return
            var methodNoArgsNoReturn = owningType.GetMethod(nameof(TestClass.MethodNoArgsNoReturn)).Compile();
            methodNoArgsNoReturn.Invoke();

            // Static method /w no args and return value
            var methodNoArgsHasReturn = owningType.GetMethod(nameof(TestClass.MethodNoArgsHasReturn)).Compile<int>();
            var value = methodNoArgsHasReturn.InvokeAndReturn();
            Console.WriteLine($"Return value = {value}");

            // Static method /w args and no return
            var methodHasArgsNoReturn = owningType.GetMethod(nameof(TestClass.MethodWithArgsAndNoReturn)).Compile();
            methodHasArgsNoReturn.Invoke(2, 5);

            // Static method /w args and return value
            var methodHasArgsHasReturn = owningType.GetMethod(nameof(TestClass.MethodWithArgsAndReturn)).Compile<int>();
            value = methodHasArgsHasReturn.InvokeAndReturn(2, 5);
            Console.WriteLine($"Return value = {value}");
        }
    }
}
