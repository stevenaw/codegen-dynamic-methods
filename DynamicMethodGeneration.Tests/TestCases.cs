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
                        Method = typeof(TestClass).GetMethod(nameof(TestClass.StaticMethodNoArgsNoReturn)),
                        Args = null
                    },
                    new TestCaseData<MethodInfo>()
                    {
                        Method = typeof(TestClass).GetMethod(nameof(TestClass.StaticMethodNoArgsNoReturn)),
                        Args = new object[0]
                    },
                    new TestCaseData<MethodInfo>()
                    {
                        Method = typeof(TestClass).GetMethod(nameof(TestClass.StaticMethodWithArgsAndNoReturn)),
                        Args = new object[]
                        {
                            2,
                            5
                        }
                    },

                    new TestCaseData<MethodInfo>()
                    {
                        Method = typeof(TestStruct).GetMethod(nameof(TestStruct.StaticMethodNoArgsNoReturn)),
                        Args = null
                    },
                    new TestCaseData<MethodInfo>()
                    {
                        Method = typeof(TestStruct).GetMethod(nameof(TestStruct.StaticMethodNoArgsNoReturn)),
                        Args = new object[0]
                    },
                    new TestCaseData<MethodInfo>()
                    {
                        Method = typeof(TestStruct).GetMethod(nameof(TestStruct.StaticMethodWithArgsAndNoReturn)),
                        Args = new object[]
                        {
                            2,
                            5
                        }
                    },

                    // Instance
                    new TestCaseData<MethodInfo>()
                    {
                        Instance = new TestClass(),
                        Method = typeof(TestClass).GetMethod(nameof(TestClass.MethodNoArgsNoReturn)),
                        Args = null
                    },
                    new TestCaseData<MethodInfo>()
                    {
                        Instance = new TestClass(),
                        Method = typeof(TestClass).GetMethod(nameof(TestClass.MethodNoArgsNoReturn)),
                        Args = new object[0]
                    },
                    new TestCaseData<MethodInfo>()
                    {
                        Instance = new TestClass(),
                        Method = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithArgsAndNoReturn)),
                        Args = new object[]
                        {
                            2,
                            5
                        }
                    },

                    new TestCaseData<MethodInfo>()
                    {
                        Instance = new TestStruct(),
                        Method = typeof(TestStruct).GetMethod(nameof(TestStruct.MethodNoArgsNoReturn)),
                        Args = null
                    },
                    new TestCaseData<MethodInfo>()
                    {
                        Instance = new TestStruct(),
                        Method = typeof(TestStruct).GetMethod(nameof(TestStruct.MethodNoArgsNoReturn)),
                        Args = new object[0]
                    },
                    new TestCaseData<MethodInfo>()
                    {
                        Instance = new TestStruct(),
                        Method = typeof(TestStruct).GetMethod(nameof(TestStruct.MethodWithArgsAndNoReturn)),
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
                        Method = typeof(TestClass).GetMethod(nameof(TestClass.StaticMethodNoArgsHasReturn)),
                        Args = null,
                        ExpectedResult = TestClass.StaticMethodNoArgsHasReturn()
                    },
                    new TestCaseData<MethodInfo>()
                    {
                        Method = typeof(TestClass).GetMethod(nameof(TestClass.StaticMethodNoArgsHasReturn)),
                        Args = new object[0],
                        ExpectedResult = TestClass.StaticMethodNoArgsHasReturn()
                    },
                    new TestCaseData<MethodInfo>()
                    {
                        Method = typeof(TestClass).GetMethod(nameof(TestClass.StaticMethodWithArgsAndReturn)),
                        Args = new object[] { 2, 5 },
                        ExpectedResult = TestClass.StaticMethodWithArgsAndReturn(2, 5)
                    },

                    new TestCaseData<MethodInfo>()
                    {
                        Method = typeof(TestStruct).GetMethod(nameof(TestStruct.StaticMethodNoArgsHasReturn)),
                        Args = null,
                        ExpectedResult = TestStruct.StaticMethodNoArgsHasReturn()
                    },
                    new TestCaseData<MethodInfo>()
                    {
                        Method = typeof(TestStruct).GetMethod(nameof(TestStruct.StaticMethodNoArgsHasReturn)),
                        Args = new object[0],
                        ExpectedResult = TestStruct.StaticMethodNoArgsHasReturn()
                    },
                    new TestCaseData<MethodInfo>()
                    {
                        Method = typeof(TestStruct).GetMethod(nameof(TestStruct.StaticMethodWithArgsAndReturn)),
                        Args = new object[] { 2, 5 },
                        ExpectedResult = TestStruct.StaticMethodWithArgsAndReturn(2, 5)
                    },

                    // Instance
                    new TestCaseData<MethodInfo>()
                    {
                        Instance = new TestClass(),
                        Method = typeof(TestClass).GetMethod(nameof(TestClass.MethodNoArgsHasReturn)),
                        Args = null,
                        ExpectedResult = new TestClass().MethodNoArgsHasReturn()
                    },
                    new TestCaseData<MethodInfo>()
                    {
                        Instance = new TestClass(),
                        Method = typeof(TestClass).GetMethod(nameof(TestClass.MethodNoArgsHasReturn)),
                        Args = new object[0],
                        ExpectedResult = new TestClass().MethodNoArgsHasReturn()
                    },
                    new TestCaseData<MethodInfo>()
                    {
                        Instance = new TestClass(),
                        Method = typeof(TestClass).GetMethod(nameof(TestClass.MethodWithArgsAndReturn)),
                        Args = new object[] { 2, 5 },
                        ExpectedResult = new TestClass().MethodWithArgsAndReturn(2, 5)
                    },

                    new TestCaseData<MethodInfo>()
                    {
                        Instance = new TestStruct(),
                        Method = typeof(TestStruct).GetMethod(nameof(TestStruct.MethodNoArgsHasReturn)),
                        Args = null,
                        ExpectedResult = new TestStruct().MethodNoArgsHasReturn()
                    },
                    new TestCaseData<MethodInfo>()
                    {
                        Instance = new TestStruct(),
                        Method = typeof(TestStruct).GetMethod(nameof(TestStruct.MethodNoArgsHasReturn)),
                        Args = new object[0],
                        ExpectedResult = new TestStruct().MethodNoArgsHasReturn()
                    },
                    new TestCaseData<MethodInfo>()
                    {
                        Instance = new TestStruct(),
                        Method = typeof(TestStruct).GetMethod(nameof(TestStruct.MethodWithArgsAndReturn)),
                        Args = new object[] { 2, 5 },
                        ExpectedResult = new TestStruct().MethodWithArgsAndReturn(2, 5)
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
                        Instance = new TestClass(),
                        Method = typeof(TestClass).GetProperty(nameof(TestClass.PropertyTest)),
                        Args = new object[] { 2 }
                    },
                    new TestCaseData<PropertyInfo>()
                    {
                        Method = typeof(TestClass).GetProperty(nameof(TestClass.StaticPropertyTest)),
                        Args = new object[] { 2 }
                    },
                    new TestCaseData<PropertyInfo>()
                    {
                        Instance = new TestStruct(),
                        Method = typeof(TestStruct).GetProperty(nameof(TestStruct.PropertyTest)),
                        Args = new object[] { 2 }
                    },
                    new TestCaseData<PropertyInfo>()
                    {
                        Method = typeof(TestStruct).GetProperty(nameof(TestStruct.StaticPropertyTest)),
                        Args = new object[] { 2 }
                    },
                };
            }
        }

        public static TestCaseData<FieldInfo>[] FieldTestCases
        {
            get
            {
                return new TestCaseData<FieldInfo>[]
                {
                    new TestCaseData<FieldInfo>()
                    {
                        Instance = new TestClass(),
                        Method = typeof(TestClass).GetField(nameof(TestClass.FieldTest)),
                        Args = new object[] { 2 }
                    },
                    new TestCaseData<FieldInfo>()
                    {
                        Method = typeof(TestClass).GetField(nameof(TestClass.StaticFieldTest)),
                        Args = new object[] { 2 }
                    },
                    new TestCaseData<FieldInfo>()
                    {
                        Instance = new TestStruct(),
                        Method = typeof(TestStruct).GetField(nameof(TestStruct.FieldTest)),
                        Args = new object[] { 2 }
                    },
                    new TestCaseData<FieldInfo>()
                    {
                        Method = typeof(TestStruct).GetField(nameof(TestStruct.StaticFieldTest)),
                        Args = new object[] { 2 }
                    },
                };
            }
        }
    }
}
