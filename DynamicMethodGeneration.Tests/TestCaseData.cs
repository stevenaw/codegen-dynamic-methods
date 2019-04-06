using System.Reflection;

namespace DynamicMethodGeneration.Tests
{
    public class TestCaseData<TMember> where TMember : MemberInfo
    {
        public object Instance { get; set; }
        public TMember Method { get; set; }
        public object[] Args { get; set; }
        public object ExpectedResult { get; set; }
    }
}
