using NUnit.Framework;

namespace DynamicMethodGeneration.Tests
{
    // TODO: Verify the expected method is actually called
    // TODO: Failure tests where invalid number of params are passed
    // TODO: Test overloaded functions
    public partial class DynamicMethodFactoryTests
    {
        [TestCaseSource(nameof(InstanceActionTestCases))]
        public void GetAction_ShouldReturnInvocable(InstanceTestCaseData testCase)
        {
            var factory = new DynamicMethodFactory();
            var method = factory.GetAction(testCase.Instance, testCase.MethodName, testCase.Args);

            Assert.That(method, Is.Not.Null);

            var methodArgs = ArrayHelper.Prepend(testCase.Args ?? new object[0], testCase.Instance);
            method.DynamicInvoke(methodArgs);
        }

        [TestCaseSource(nameof(InstanceFunctionTestCases))]
        public void GetFunction_ShouldReturnInvocable(InstanceTestCaseData testCase)
        {
            var factory = new DynamicMethodFactory();
            var method = factory.GetFunction<int>(testCase.Instance, testCase.MethodName, testCase.Args);

            Assert.That(method, Is.Not.Null);

            var methodArgs = ArrayHelper.Prepend(testCase.Args ?? new object[0], testCase.Instance);
            var result = (int)method.DynamicInvoke(methodArgs);

            Assert.That(result, Is.EqualTo(testCase.ExpectedResult));
        }

        [TestCaseSource(nameof(StaticActionTestCases))]
        public void GetAction_ShouldReturnInvocable(StaticTestCaseData testCase)
        {
            var factory = new DynamicMethodFactory();
            var method = factory.GetAction(testCase.Type, testCase.MethodName, testCase.Args);

            Assert.That(method, Is.Not.Null);

            method.DynamicInvoke(testCase.Args);
        }

        [TestCaseSource(nameof(StaticFunctionTestCases))]
        public void GetFunction_ShouldReturnInvocable(StaticTestCaseData testCase)
        {
            var factory = new DynamicMethodFactory();
            var method = factory.GetFunction<int>(testCase.Type, testCase.MethodName, testCase.Args);

            Assert.That(method, Is.Not.Null);

            var result = (int)method.DynamicInvoke(testCase.Args);

            Assert.That(result, Is.EqualTo(testCase.ExpectedResult));
        }

        #region TestCaseData
        public static StaticTestCaseData[] StaticActionTestCases
        {
            get
            {
                return new StaticTestCaseData[]
                {
                    new StaticTestCaseData()
                    {
                        Type = typeof(TestStaticClass),
                        MethodName = nameof(TestStaticClass.MethodNoArgsNoReturn),
                        Args = null
                    },
                    new StaticTestCaseData()
                    {
                        Type = typeof(TestStaticClass),
                        MethodName = nameof(TestStaticClass.MethodNoArgsNoReturn),
                        Args = new object[0]
                    },
                    new StaticTestCaseData()
                    {
                        Type = typeof(TestStaticClass),
                        MethodName = nameof(TestStaticClass.MethodWithArgsAndNoReturn),
                        Args = new object[]
                        {
                            2,
                            5
                        }
                    }
                };
            }
        }

        public static StaticTestCaseData[] StaticFunctionTestCases
        {
            get
            {
                return new StaticTestCaseData[]
                {
                    new StaticTestCaseData()
                    {
                        Type = typeof(TestStaticClass),
                        MethodName = nameof(TestStaticClass.MethodNoArgsHasReturn),
                        Args = null,
                        ExpectedResult = TestStaticClass.MethodNoArgsHasReturn()
                    },
                    new StaticTestCaseData()
                    {
                        Type = typeof(TestStaticClass),
                        MethodName = nameof(TestStaticClass.MethodNoArgsHasReturn),
                        Args = new object[0],
                        ExpectedResult = TestStaticClass.MethodNoArgsHasReturn()
                    },
                    new StaticTestCaseData()
                    {
                        Type = typeof(TestStaticClass),
                        MethodName = nameof(TestStaticClass.MethodWithArgsAndReturn),
                        Args = new object[] { 2, 5 },
                        ExpectedResult = TestStaticClass.MethodWithArgsAndReturn(2, 5)
                    }
                };
            }
        }

        public static InstanceTestCaseData[] InstanceActionTestCases
        {
            get
            {
                return new InstanceTestCaseData[]
                {
                    new InstanceTestCaseData()
                    {
                        Instance = new TestInstanceClass(),
                        MethodName = nameof(TestInstanceClass.MethodNoArgsNoReturn),
                        Args = null
                    },
                    new InstanceTestCaseData()
                    {
                        Instance = new TestInstanceClass(),
                        MethodName = nameof(TestInstanceClass.MethodNoArgsNoReturn),
                        Args = new object[0]
                    },
                    new InstanceTestCaseData()
                    {
                        Instance = new TestInstanceClass(),
                        MethodName = nameof(TestInstanceClass.MethodWithArgsAndNoReturn),
                        Args = new object[]
                        {
                            2,
                            5
                        }
                    }
                };
            }
        }

        public static InstanceTestCaseData[] InstanceFunctionTestCases
        {
            get
            {
                return new InstanceTestCaseData[]
                {
                    new InstanceTestCaseData()
                    {
                        Instance = new TestInstanceClass(),
                        MethodName = nameof(TestInstanceClass.MethodNoArgsHasReturn),
                        Args = null,
                        ExpectedResult = new TestInstanceClass().MethodNoArgsHasReturn()
                    },
                    new InstanceTestCaseData()
                    {
                        Instance = new TestInstanceClass(),
                        MethodName = nameof(TestInstanceClass.MethodNoArgsHasReturn),
                        Args = new object[0],
                        ExpectedResult = new TestInstanceClass().MethodNoArgsHasReturn()
                    },
                    new InstanceTestCaseData()
                    {
                        Instance = new TestInstanceClass(),
                        MethodName = nameof(TestInstanceClass.MethodWithArgsAndReturn),
                        Args = new object[] { 2, 5 },
                        ExpectedResult = new TestInstanceClass().MethodWithArgsAndReturn(2, 5)
                    }
                };
            }
        }
        #endregion
    }
}
