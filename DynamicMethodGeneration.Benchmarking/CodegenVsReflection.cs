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

        private MethodInfo _methodForGenerating;
        private MethodInfo _methodForCaching;

        [GlobalSetup]
        public void InitialSetup()
        {
            _testClass = new TestClass();

            _methodForGenerating = _testClass.GetType().GetMethod(nameof(TestClass.MethodForGenerating));
            _methodForCaching = _testClass.GetType().GetMethod(nameof(TestClass.MethodForCaching));

            _invoker = new DynamicMethodInvoker();

            // Setup internal cache for this method early so it doesn't weigh it all down
            _invoker.InvokeAction(_methodForCaching, _testClass, 2, 4);
        }

        [Benchmark]
        public void CodegenBareInvoke() => _invoker.InvokeAction(_methodForGenerating, _testClass, 2, 4);

        [Benchmark]
        public void CodegenBareInvoke_WithPregen() => _invoker.InvokeAction(_methodForCaching, _testClass, 2, 4);

        [Benchmark]
        public void ReflectionBareInvoke() => _methodForGenerating.Invoke(_testClass, new object[] { 2, 4 });

        public class TestClass
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public void MethodForGenerating(int a, int b)
            {
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            public void MethodForCaching(int a, int b)
            {
            }
        }
    }
}
