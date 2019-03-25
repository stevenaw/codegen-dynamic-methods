namespace DynamicMethodGeneration.Tests
{
    public class InstanceTestCaseData
    {
        public object Instance { get; set; }
        public string MethodName { get; set; }
        public object[] Args { get; set; }
        public object ExpectedResult { get; set; }
    }
}
