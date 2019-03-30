using System.Reflection;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace DynamicMethodGeneration.Benchmarking
{
    [InProcess(dontLogOutput: true)]
    public class CodegenVsReflection
    {
        private TestClass _testClass;

        private MethodInfo _methodForGenerating;
        private MethodInfo _methodForCaching;
        private DynamicMethod _cachedCompiledMethod;

        [GlobalSetup]
        public void InitialSetup()
        {
            _testClass = new TestClass();

            _methodForGenerating = _testClass.GetType().GetMethod(nameof(TestClass.MethodForGenerating));
            _methodForCaching = _testClass.GetType().GetMethod(nameof(TestClass.MethodForCaching));
            
            _cachedCompiledMethod = _methodForCaching.Compile();
        }

        [Benchmark]
        public void Codegen_GenerateAndInvoke() => _methodForGenerating
                                                    .Compile()
                                                    .WithInstance(_testClass)
                                                    .Invoke(2, 4);

        [Benchmark]
        public void Codegen_BareInvoke() => _cachedCompiledMethod
                                                .WithInstance(_testClass)
                                                .Invoke(2, 4);

        [Benchmark]
        public void Reflection_BareInvoke() => _methodForGenerating.Invoke(_testClass, new object[] { 2, 4 });

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
