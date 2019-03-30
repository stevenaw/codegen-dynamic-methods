using System;

namespace DynamicMethodGeneration.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TestStaticFunctions();
            TestInstanceFunctions();

            Console.Write("Please press a key...");
            Console.Read();
        }

        private static void TestInstanceFunctions()
        {
            var target = new TestInstanceClass();
            var owningType = typeof(TestInstanceClass);

            // Static method /w no args and no return
            var methodNoArgsNoReturn = owningType.GetMethod(nameof(TestInstanceClass.MethodNoArgsNoReturn)).Compile();
            methodNoArgsNoReturn.Invoke(target);

            // Static method /w no args and return value
            var methodNoArgsHasReturn = owningType.GetMethod(nameof(TestInstanceClass.MethodNoArgsHasReturn)).Compile<int>();
            var value = methodNoArgsHasReturn.Invoke(target);
            Console.WriteLine($"Return value = {value}");

            // Static method /w args and no return
            var methodHasArgsNoReturn = owningType.GetMethod(nameof(TestInstanceClass.MethodWithArgsAndNoReturn)).Compile();
            methodHasArgsNoReturn.Invoke(target, 2, 5);

            // Static method /w args and return value
            var methodHasArgsHasReturn = owningType.GetMethod(nameof(TestInstanceClass.MethodWithArgsAndReturn)).Compile<int>();
            value = methodHasArgsHasReturn.Invoke(target, 2, 5);
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
            var value = methodNoArgsHasReturn.Invoke();
            Console.WriteLine($"Return value = {value}");

            // Static method /w args and no return
            var methodHasArgsNoReturn = owningType.GetMethod(nameof(TestClass.MethodWithArgsAndNoReturn)).Compile();
            methodHasArgsNoReturn.Invoke(2, 5);

            // Static method /w args and return value
            var methodHasArgsHasReturn = owningType.GetMethod(nameof(TestClass.MethodWithArgsAndReturn)).Compile<int>();
            value = methodHasArgsHasReturn.Invoke(2, 5);
            Console.WriteLine($"Return value = {value}");
        }
    }
}
