using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicMethodGeneration.ConsoleApp
{
    public class TestInstanceClass
    {
        public int MethodWithArgsAndReturn(int a, int b)
        {
            return a * b;
        }
        public void MethodWithArgsAndNoReturn(int a, int b)
        {
            Console.WriteLine($"Output is {a + b}");
        }

        public int MethodNoArgsHasReturn()
        {
            return 42;
        }

        public void MethodNoArgsNoReturn()
        {
            Console.WriteLine("Hello World!");
        }
    }
}
