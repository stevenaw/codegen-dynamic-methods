using System;
using System.Collections.Generic;

namespace DynamicMethodGeneration.Tests
{
    public class TestInstanceClass
    {
        public int FieldTest;

        private Dictionary<string, string> _indexerBackingField = new Dictionary<string, string>();
        public string this[string key]
        {
            get { return _indexerBackingField[key]; }
            set { _indexerBackingField[key] = value; }
        }

        public int PropertyWithoutArgument { get; set; }

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
