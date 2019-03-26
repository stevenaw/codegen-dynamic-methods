using System;
using BenchmarkDotNet.Running;

namespace DynamicMethodGeneration.Benchmarking
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<CodegenVsReflection>();

            Console.Read();
        }
    }
}
