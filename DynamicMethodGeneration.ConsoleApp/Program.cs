using System;
using DynamicMethodGeneration.Extensions;

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

            target.InvokeAction(nameof(TestInstanceClass.MethodNoArgsNoReturn));

            var value = target.InvokeFunction<int>(nameof(TestInstanceClass.MethodNoArgsHasReturn));
            Console.WriteLine($"Return value = {value}");

            target.InvokeAction(nameof(TestInstanceClass.MethodWithArgsAndNoReturn), 2, 5);

            value = target.InvokeFunction<int>(nameof(TestInstanceClass.MethodWithArgsAndReturn), 2, 5);
            Console.WriteLine($"Return value = {value}");
        }

        private static void TestStaticFunctions()
        {
            var owningType = typeof(TestClass);
            var invoker = new DynamicMethodInvoker();

            // Static method /w no args and no return
            invoker.InvokeAction(owningType.GetMethod(nameof(TestClass.MethodNoArgsNoReturn)));

            // Static method /w no args and return value
            var value = invoker.InvokeFunction<int>(owningType.GetMethod(nameof(TestClass.MethodNoArgsHasReturn)));
            Console.WriteLine($"Return value = {value}");

            // Static method /w args and no return
            invoker.InvokeAction(owningType.GetMethod(nameof(TestClass.MethodWithArgsAndNoReturn)), null, 2, 5);

            // Static method /w args and return value
            value = invoker.InvokeFunction<int>(owningType.GetMethod(nameof(TestClass.MethodWithArgsAndReturn)), null, 2, 5);
            Console.WriteLine($"Return value = {value}");
        }
    }
}
