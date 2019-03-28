using System.Reflection;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace DynamicMethodGeneration.Benchmarking
{
    [InProcess(dontLogOutput: true)]
    public class CodegenVsReflection
    {
        private TestClass _testClass;
        private DynamicMethodInvoker _invoker;
        private MethodInfo _methodReflection;

        [GlobalSetup]
        public void InitialSetup()
        {
            _testClass = new TestClass();
            _methodReflection = _testClass.GetType().GetMethod(nameof(TestClass.MyMethod));

            _invoker = new DynamicMethodInvoker();
            
        }

        [Benchmark]
        public void CodegenBareInvoke() => _invoker.InvokeAction(_methodReflection, _testClass, 2, 4);
        
        [Benchmark]
        public void ReflectionBareInvoke()
        {
            _methodReflection.Invoke(_testClass, new object[] { 2, 4 });
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
