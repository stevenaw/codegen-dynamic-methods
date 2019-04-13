namespace DynamicMethodGeneration.Tests
{
    internal static class TestCaseHelper
    {
        public static object[] GetArgs(object[] args, object instance)
        {
            if (instance == null)
                return args ?? new object[0];
            else if (args == null)
                return new object[] { instance };
            else
                return ArrayHelper.Prepend(args, instance);
        }
    }
}
