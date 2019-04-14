using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;

namespace DynamicMethodGeneration.Tests
{
    // TODO: Verify the expected method is actually called
    // TODO: Failure tests where invalid number of params are passed
    public partial class DynamicMethodFactoryTests
    {
        [TestCaseSource(typeof(TestCases), nameof(TestCases.ActionTestCases))]
        public void GetAction_ShouldReturnInvocable(TestCaseData<MethodInfo> testCase)
        {
            var factory = new DynamicMethodFactory();
            var methodRequest  = DynamicMethodRequest.MakeRequest(testCase.Method);
            var method = factory.GetAction(methodRequest);

            Assert.That(method, Is.Not.Null);

            var methodArgs = TestCaseHelper.GetArgs(testCase.Args, testCase.Instance);
            method.Invoker.DynamicInvoke(methodArgs);
        }

        [TestCaseSource(typeof(TestCases), nameof(TestCases.FunctionTestCases))]
        public void GetFunction_ShouldReturnInvocable(TestCaseData<MethodInfo> testCase)
        {
            var factory = new DynamicMethodFactory();
            var methodRequest = DynamicMethodRequest.MakeRequest(testCase.Method);
            var method = factory.GetFunction<int>(methodRequest);

            Assert.That(method, Is.Not.Null);

            var methodArgs = TestCaseHelper.GetArgs(testCase.Args, testCase.Instance);
            var result = (int)method.Invoker.DynamicInvoke(methodArgs);

            Assert.That(result, Is.EqualTo(testCase.ExpectedResult));
        }



        [Test]
        public void GenerateProperty_ShouldSetAndGetIndexer()
        {
            const string expectedKey = "Hello";
            const string expectedValue = "World";

            var factory = new DynamicMethodFactory();
            var instance = new TestClass();
            var member = typeof(TestClass)
                .GetProperties()
                .First(o => o.GetIndexParameters().Any());

            var setRequest = DynamicMethodRequest.MakeRequest(member.SetMethod);
            var setMethod = factory.GetAction(setRequest);
            setMethod.Invoke(instance, expectedKey, expectedValue);

            var getRequest = DynamicMethodRequest.MakeRequest(member.GetMethod);
            var getMethod = factory.GetFunction<string>(getRequest);
            var value = getMethod.Invoke(instance, expectedKey);

            Assert.That(value, Is.EqualTo(expectedValue));
        }

        [Test]
        public void GenerateProperty_ShouldGetValue()
        {
            var factory = new DynamicMethodFactory();
            var instance = new TestClass() { PropertyTest = 3 };
            var member = typeof(TestClass).GetProperty(nameof(TestClass.PropertyTest));

            var getRequest = DynamicMethodRequest.MakeRequest(member.GetMethod);
            var getMethod = factory.GetFunction<int>(getRequest);
            var methodFunc = (Func<TestClass, int>)getMethod.Invoker;

            var result = methodFunc(instance);
            Assert.That(result, Is.EqualTo(instance.PropertyTest));
        }

        [Test]
        public void GenerateProperty_ShouldSetValue()
        {
            var factory = new DynamicMethodFactory();
            var instance = new TestClass();
            var member = typeof(TestClass).GetProperty(nameof(TestClass.PropertyTest));

            var setRequest = DynamicMethodRequest.MakeRequest(member.SetMethod);

            var setMethod = factory.GetAction(setRequest);
            var methodFunc = (Action<TestClass, int>)setMethod.Invoker;

            methodFunc(instance, 3);
            Assert.That(instance.PropertyTest, Is.EqualTo(3));
        }

        [TestCaseSource(typeof(TestCases), nameof(TestCases.PropertyTestCases))]
        public void GenerateProperty_ShouldBeBidirectional(TestCaseData<PropertyInfo> testCase)
        {
            var factory = new DynamicMethodFactory();
            var expectedResult = testCase.Args.Last();

            var setRequest = DynamicMethodRequest.MakeRequest(testCase.Method.SetMethod);
            var setMethod = factory.GetAction(setRequest);
            var setArgs = TestCaseHelper.GetArgs(testCase.Args, testCase.Instance);
            setMethod.Invoke(setArgs);

            var getRequest = DynamicMethodRequest.MakeRequest(testCase.Method.GetMethod);
            var getMethod = factory.GetFunction<int>(getRequest);
            var getArgs = TestCaseHelper.GetArgs(null, testCase.Instance);
            var result = getMethod.Invoke(getArgs);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GenerateField_ShouldGetValue()
        {
            var factory = new DynamicMethodFactory();
            var instance = new TestClass() { FieldTest = 3 };
            var member = typeof(TestClass).GetField(nameof(TestClass.FieldTest));

            var getRequest = DynamicMethodRequest.MakeGetterRequest(member);
            var getMethod = factory.GetFunction<int>(getRequest);
            var methodFunc = (Func<TestClass, int>)getMethod.Invoker;

            var result = methodFunc(instance);
            Assert.That(result, Is.EqualTo(instance.FieldTest));
        }

        [Test]
        public void GenerateField_ShouldSetValue()
        {
            var factory = new DynamicMethodFactory();
            var instance = new TestClass();
            var member = typeof(TestClass).GetField(nameof(TestClass.FieldTest));

            var setRequest = DynamicMethodRequest.MakeSetterRequest(member);
            var setMethod = factory.GetAction(setRequest);
            var methodFunc = (Action<TestClass, int>)setMethod.Invoker;

            methodFunc(instance, 3);
            Assert.That(instance.FieldTest, Is.EqualTo(3));
        }

        [TestCaseSource(typeof(TestCases), nameof(TestCases.FieldTestCases))]
        public void GenerateField_ShouldBeBidirectional(TestCaseData<FieldInfo> testCase)
        {
            var factory = new DynamicMethodFactory();
            var expectedResult = testCase.Args.Last();

            var setRequest = DynamicMethodRequest.MakeSetterRequest(testCase.Method);
            var setMethod = factory.GetAction(setRequest);
            var setArgs = TestCaseHelper.GetArgs(testCase.Args, testCase.Instance);
            setMethod.Invoke(setArgs);

            var getRequest = DynamicMethodRequest.MakeGetterRequest(testCase.Method);
            var getMethod = factory.GetFunction<int>(getRequest);
            var getArgs = TestCaseHelper.GetArgs(null, testCase.Instance);
            var result = getMethod.Invoke(getArgs);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GenerateDelegate_ShouldFail_WhenTooManyParameters()
        {
            var factory = new DynamicMethodFactory();
            var methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.MethodTooManyParameters));
            var methodRequest = DynamicMethodRequest.MakeRequest(methodInfo);

            Assert.That(
                () => factory.GetAction(methodRequest),
                Throws.InvalidOperationException
            );
        }
    }
}
