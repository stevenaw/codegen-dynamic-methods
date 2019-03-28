using System.Reflection;

namespace DynamicMethodGeneration.Tests
{
    public class TestCaseData
    {
        public object Instance { get; set; }
        public MethodInfo Method { get; set; }
        public object[] Args { get; set; }
        public object ExpectedResult { get; set; }
    }
}
