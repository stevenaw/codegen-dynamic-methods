using System;

namespace DynamicMethodGeneration.Tests
{
    public class TestStruct
    {
        public int FieldTest;
        public int PropertyTest { get; set; }

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


        public static int StaticFieldTest;

        public static int StaticPropertyTest { get; set; }

        public static int StaticMethodWithArgsAndReturn(int a, int b)
        {
            return a * b;
        }

        public static void StaticMethodWithArgsAndNoReturn(int a, int b)
        {
            Console.WriteLine($"Output is {a + b}");
        }

        public static int StaticMethodNoArgsHasReturn()
        {
            return 42;
        }

        public static void StaticMethodNoArgsNoReturn()
        {
            Console.WriteLine("Hello World!");
        }
    }
}
