using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using DynamicMethodGeneration.Extensions;

namespace DynamicMethodGeneration.Benchmarking
{
    [InProcess(dontLogOutput: true)]
    public class CodegenVsReflection
    {
        private TestClass _testClass;
        private Action<TestClass, int, int> _method;
        private MethodInfo _methodReflection;

        [GlobalSetup]
        public void InitialSetup()
        {
            var factory = new DynamicMethodFactory();

            _testClass = new TestClass();
            _method = (Action<TestClass, int, int>)factory.GetAction(_testClass, nameof(TestClass.MyMethod), 2, 4);
            _methodReflection = _testClass.GetType().GetMethod(nameof(TestClass.MyMethod));
        }

        [Benchmark]
        public void CodegenBareInvoke() => _method(_testClass, 2, 4);

        [Benchmark]
        public void CodeGenColdStart() => _testClass.InvokeAction(nameof(TestClass.MyMethod), 2, 4);

        [Benchmark]
        public void ReflectionColdStart()
        {
            _testClass.GetType().GetMethod(nameof(TestClass.MyMethod)).Invoke(_testClass, new object[] { 2, 2 });
        }

        [Benchmark]
        public void ReflectionBareInvoke()
        {
            _methodReflection.Invoke(_testClass, new object[] { 2, 2 });
        }

        public class TestClass
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public void MyMethod(int a, int b)
            {
            }
        }
    }
}
