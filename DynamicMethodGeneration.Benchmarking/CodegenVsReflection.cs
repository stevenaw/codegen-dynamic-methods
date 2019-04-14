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
        private DynamicMethodInvocation<TestClass> _cachedCompiledMethodWithInstance;

        [GlobalSetup]
        public void InitialSetup()
        {
            _testClass = new TestClass();

            _methodForGenerating = _testClass.GetType().GetMethod(nameof(TestClass.MethodForGenerating));
            _methodForCaching = _testClass.GetType().GetMethod(nameof(TestClass.MethodForCaching));
            
            _cachedCompiledMethod = _methodForCaching.Compile();
            _cachedCompiledMethodWithInstance = _cachedCompiledMethod.WithInstance(_testClass);
        }

        [Benchmark]
        public void Codegen_GenerateAndInvoke() => _methodForGenerating
                                                    .Compile()
                                                    .WithInstance(_testClass)
                                                    .Invoke(2, 4);

        [Benchmark]
        public void Codegen_BareInvoke() => _cachedCompiledMethodWithInstance
                                                .Invoke(2, 4);

        [Benchmark]
        public void Codegen_BareInvoke_WithInstance() => _cachedCompiledMethod
                                                .WithInstance(_testClass)
                                                .Invoke(2, 4);

        [Benchmark]
        public void Reflection_BareInvoke() => _methodForGenerating.Invoke(_testClass, new object[] { 2, 4 });

        public class TestClass
        {
            public void MethodForGenerating(int a, int b)
            {
            }

            public void MethodForCaching(int a, int b)
            {
            }
        }
    }
}
