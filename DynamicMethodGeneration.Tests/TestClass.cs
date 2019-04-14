using System;
using System.Collections.Generic;

namespace DynamicMethodGeneration.Tests
{
    public class TestClass
    {
        public int FieldTest;

        private Dictionary<string, string> _indexerBackingField = new Dictionary<string, string>();
        public string this[string key]
        {
            get { return _indexerBackingField[key]; }
            set { _indexerBackingField[key] = value; }
        }

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


        public void MethodTooManyParameters(
            int a1,
            int a2,
            int a3,
            int a4,
            int a5,
            int a6,
            int a7,
            int a8,
            int a9
        )
        {
            Console.WriteLine("Inconceivable!");
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
