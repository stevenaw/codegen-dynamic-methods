using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DynamicMethodGeneration.Tests
{
    internal class TestCases
    {
        public static TestCaseData<MethodInfo>[] ActionTestCases
        {
            get
            {
                return new TestCaseData<MethodInfo>[]
                {
                    // Static
                    new TestCaseData<MethodInfo>()
                    {
                        Method = typeof(TestStaticClass).GetMethod(nameof(TestStaticClass.MethodNoArgsNoReturn)),
                        Args = null
                    },
                    new TestCaseData<MethodInfo>()
                    {
                        Method = typeof(TestStaticClass).GetMethod(nameof(TestStaticClass.MethodNoArgsNoReturn)),
                        Args = new object[0]
                    },
                    new TestCaseData<MethodInfo>()
                    {
                        Method = typeof(TestStaticClass).GetMethod(nameof(TestStaticClass.MethodWithArgsAndNoReturn)),
                        Args = new object[]
                        {
                            2,
                            5
                        }
                    },

                    // Instance
                    new TestCaseData<MethodInfo>()
                    {
                        Instance = new TestInstanceClass(),
                        Method = typeof(TestInstanceClass).GetMethod(nameof(TestInstanceClass.MethodNoArgsNoReturn)),
                        Args = null
                    },
                    new TestCaseData<MethodInfo>()
                    {
                        Instance = new TestInstanceClass(),
                        Method = typeof(TestInstanceClass).GetMethod(nameof(TestInstanceClass.MethodNoArgsNoReturn)),
                        Args = new object[0]
                    },
                    new TestCaseData<MethodInfo>()
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

        public static TestCaseData<MethodInfo>[] FunctionTestCases
        {
            get
            {
                return new TestCaseData<MethodInfo>[]
                {
                    // Static
                    new TestCaseData<MethodInfo>()
                    {
                        Method = typeof(TestStaticClass).GetMethod(nameof(TestStaticClass.MethodNoArgsHasReturn)),
                        Args = null,
                        ExpectedResult = TestStaticClass.MethodNoArgsHasReturn()
                    },
                    new TestCaseData<MethodInfo>()
                    {
                        Method = typeof(TestStaticClass).GetMethod(nameof(TestStaticClass.MethodNoArgsHasReturn)),
                        Args = new object[0],
                        ExpectedResult = TestStaticClass.MethodNoArgsHasReturn()
                    },
                    new TestCaseData<MethodInfo>()
                    {
                        Method = typeof(TestStaticClass).GetMethod(nameof(TestStaticClass.MethodWithArgsAndReturn)),
                        Args = new object[] { 2, 5 },
                        ExpectedResult = TestStaticClass.MethodWithArgsAndReturn(2, 5)
                    },

                    // Instance
                    new TestCaseData<MethodInfo>()
                    {
                        Instance = new TestInstanceClass(),
                        Method = typeof(TestInstanceClass).GetMethod(nameof(TestInstanceClass.MethodNoArgsHasReturn)),
                        Args = null,
                        ExpectedResult = new TestInstanceClass().MethodNoArgsHasReturn()
                    },
                    new TestCaseData<MethodInfo>()
                    {
                        Instance = new TestInstanceClass(),
                        Method = typeof(TestInstanceClass).GetMethod(nameof(TestInstanceClass.MethodNoArgsHasReturn)),
                        Args = new object[0],
                        ExpectedResult = new TestInstanceClass().MethodNoArgsHasReturn()
                    },
                    new TestCaseData<MethodInfo>()
                    {
                        Instance = new TestInstanceClass(),
                        Method = typeof(TestInstanceClass).GetMethod(nameof(TestInstanceClass.MethodWithArgsAndReturn)),
                        Args = new object[] { 2, 5 },
                        ExpectedResult = new TestInstanceClass().MethodWithArgsAndReturn(2, 5)
                    }
                };
            }
        }

        public static TestCaseData<PropertyInfo>[] PropertyTestCases
        {
            get
            {
                return new TestCaseData<PropertyInfo>[]
                {
                    new TestCaseData<PropertyInfo>()
                    {
                        Instance = new TestInstanceClass(),
                        Method = typeof(TestInstanceClass).GetProperty(nameof(TestInstanceClass.PropertyWithoutArgument)),
                        Args = new object[] { 2 }
                    }
                };
            }
        }
    }
}
