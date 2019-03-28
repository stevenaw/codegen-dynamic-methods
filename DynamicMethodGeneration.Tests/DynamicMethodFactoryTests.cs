using NUnit.Framework;

namespace DynamicMethodGeneration.Tests
{
    // TODO: Verify the expected method is actually called
    // TODO: Failure tests where invalid number of params are passed
    public partial class DynamicMethodFactoryTests
    {
        [TestCaseSource(nameof(ActionTestCases))]
        public void GetAction_ShouldReturnInvocable(TestCaseData testCase)
        {
            var factory = new DynamicMethodFactory();
            var method = factory.GetAction(testCase.Method, testCase.Instance, testCase.Args);

            Assert.That(method, Is.Not.Null);

            var methodArgs = GetArgs(testCase.Args, testCase.Instance);
            method.Invoker.DynamicInvoke(methodArgs);
        }

        [TestCaseSource(nameof(FunctionTestCases))]
        public void GetFunction_ShouldReturnInvocable(TestCaseData testCase)
        {
            var factory = new DynamicMethodFactory();
            var method = factory.GetFunction<int>(testCase.Method, testCase.Instance, testCase.Args);

            Assert.That(method, Is.Not.Null);

            var methodArgs = GetArgs(testCase.Args, testCase.Instance);
            var result = (int)method.Invoker.DynamicInvoke(methodArgs);

            Assert.That(result, Is.EqualTo(testCase.ExpectedResult));
        }

        private static object[] GetArgs(object[] args, object instance)
        {
            if (instance == null)
                return args ?? new object[0];
            else if (args == null)
                return new object[] { instance };
            else
                return ArrayHelper.Prepend(args, instance);
        }

        public static TestCaseData[] ActionTestCases
        {
            get
            {
                return new TestCaseData[]
                {
                    // Static
                    new TestCaseData()
                    {
                        Method = typeof(TestStaticClass).GetMethod(nameof(TestStaticClass.MethodNoArgsNoReturn)),
                        Args = null
                    },
                    new TestCaseData()
                    {
                        Method = typeof(TestStaticClass).GetMethod(nameof(TestStaticClass.MethodNoArgsNoReturn)),
                        Args = new object[0]
                    },
                    new TestCaseData()
                    {
                        Method = typeof(TestStaticClass).GetMethod(nameof(TestStaticClass.MethodWithArgsAndNoReturn)),
                        Args = new object[]
                        {
                            2,
                            5
                        }
                    },

                    // Instance
                    new TestCaseData()
                    {
                        Instance = new TestInstanceClass(),
                        Method = typeof(TestInstanceClass).GetMethod(nameof(TestInstanceClass.MethodNoArgsNoReturn)),
                        Args = null
                    },
                    new TestCaseData()
                    {
                        Instance = new TestInstanceClass(),
                        Method = typeof(TestInstanceClass).GetMethod(nameof(TestInstanceClass.MethodNoArgsNoReturn)),
                        Args = new object[0]
                    },
                    new TestCaseData()
                    {
                        Instance = new TestInstanceClass(),
                        Method = typeof(TestInstanceClass).GetMethod(nameof(TestInstanceClass.MethodWithArgsAndNoReturn)),
                        Args = new object[]
                        {
                            2,
                            5
                        }
                    }
                };
            }
        }

        public static TestCaseData[] FunctionTestCases
        {
            get
            {
                return new TestCaseData[]
                {
                    // Static
                    new TestCaseData()
                    {
                        Method = typeof(TestStaticClass).GetMethod(nameof(TestStaticClass.MethodNoArgsHasReturn)),
                        Args = null,
                        ExpectedResult = TestStaticClass.MethodNoArgsHasReturn()
                    },
                    new TestCaseData()
                    {
                        Method = typeof(TestStaticClass).GetMethod(nameof(TestStaticClass.MethodNoArgsHasReturn)),
                        Args = new object[0],
                        ExpectedResult = TestStaticClass.MethodNoArgsHasReturn()
                    },
                    new TestCaseData()
                    {
                        Method = typeof(TestStaticClass).GetMethod(nameof(TestStaticClass.MethodWithArgsAndReturn)),
                        Args = new object[] { 2, 5 },
                        ExpectedResult = TestStaticClass.MethodWithArgsAndReturn(2, 5)
                    },

                    // Instance
                    new TestCaseData()
                    {
                        Instance = new TestInstanceClass(),
                        Method = typeof(TestInstanceClass).GetMethod(nameof(TestInstanceClass.MethodNoArgsHasReturn)),
                        Args = null,
                        ExpectedResult = new TestInstanceClass().MethodNoArgsHasReturn()
                    },
                    new TestCaseData()
                    {
                        Instance = new TestInstanceClass(),
                        Method = typeof(TestInstanceClass).GetMethod(nameof(TestInstanceClass.MethodNoArgsHasReturn)),
                        Args = new object[0],
                        ExpectedResult = new TestInstanceClass().MethodNoArgsHasReturn()
                    },
                    new TestCaseData()
                    {
                        Instance = new TestInstanceClass(),
                        Method = typeof(TestInstanceClass).GetMethod(nameof(TestInstanceClass.MethodWithArgsAndReturn)),
                        Args = new object[] { 2, 5 },
                        ExpectedResult = new TestInstanceClass().MethodWithArgsAndReturn(2, 5)
                    }
                };
            }
        }
    }
}
