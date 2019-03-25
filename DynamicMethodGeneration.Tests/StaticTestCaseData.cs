using System;

namespace DynamicMethodGeneration.Tests
{
    public class StaticTestCaseData
    {
        public Type Type { get; set; }
        public string MethodName { get; set; }
        public object[] Args { get; set; }
        public object ExpectedResult { get; set; }
    }
}
