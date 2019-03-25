using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicMethodGeneration.ConsoleApp
{
    public static class TestClass
    {
        public static int MethodWithArgsAndReturn(int a, int b)
        {
            return a * b;
        }

        public static void MethodWithArgsAndNoReturn(int a, int b)
        {
            Console.WriteLine($"Output is {a + b}");
        }

        public static int MethodNoArgsHasReturn()
        {
            return 42;
        }

        public static void MethodNoArgsNoReturn()
        {
            Console.WriteLine("Hello World!");
        }
    }
}
