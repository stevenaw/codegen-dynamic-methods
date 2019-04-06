using NUnit.Framework;
using System;
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

            var methodArgs = TestHelper.GetArgs(testCase.Args, testCase.Instance);
            method.Invoker.DynamicInvoke(methodArgs);
        }

        [TestCaseSource(typeof(TestCases), nameof(TestCases.FunctionTestCases))]
        public void GetFunction_ShouldReturnInvocable(TestCaseData<MethodInfo> testCase)
        {
            var factory = new DynamicMethodFactory();
            var methodRequest = DynamicMethodRequest.MakeRequest(testCase.Method);
            var method = factory.GetFunction<int>(methodRequest);

            Assert.That(method, Is.Not.Null);

            var methodArgs = TestHelper.GetArgs(testCase.Args, testCase.Instance);
            var result = (int)method.Invoker.DynamicInvoke(methodArgs);

            Assert.That(result, Is.EqualTo(testCase.ExpectedResult));
        }



        [Test]
        public void GenerateProperty_ShouldGetValue()
        {
            var factory = new DynamicMethodFactory();
            var instance = new TestInstanceClass() { PropertyWithoutArgument = 3 };
            var member = typeof(TestInstanceClass).GetProperty(nameof(TestInstanceClass.PropertyWithoutArgument));

            var getRequest = DynamicMethodRequest.MakeRequest(member.GetMethod);
            var getMethod = factory.GetFunction<int>(getRequest);
            var methodFunc = (Func<TestInstanceClass, int>)getMethod.Invoker;

            var result = methodFunc(instance);
            Assert.That(result, Is.EqualTo(instance.PropertyWithoutArgument));
        }

        [Test]
        public void GenerateProperty_ShouldSetValue()
        {
            var factory = new DynamicMethodFactory();
            var instance = new TestInstanceClass();
            var member = typeof(TestInstanceClass).GetProperty(nameof(TestInstanceClass.PropertyWithoutArgument));

            var setRequest = DynamicMethodRequest.MakeRequest(member.SetMethod);

            var setMethod = factory.GetAction(setRequest);
            var methodFunc = (Action<TestInstanceClass, int>)setMethod.Invoker;

            methodFunc(instance, 3);
            Assert.That(instance.PropertyWithoutArgument, Is.EqualTo(3));
        }

        [TestCaseSource(typeof(TestCases), nameof(TestCases.PropertyTestCases))]
        public void GenerateProperty_ShouldBeBidirectional(TestCaseData<PropertyInfo> testCase)
        {
            var factory = new DynamicMethodFactory();
            var expectedResult = (int)testCase.Args[0];

            var setRequest = DynamicMethodRequest.MakeRequest(testCase.Method.SetMethod);
            var setMethod = factory.GetAction(setRequest);
            setMethod.Invoke((TestInstanceClass)testCase.Instance, expectedResult);

            var getRequest = DynamicMethodRequest.MakeRequest(testCase.Method.GetMethod);
            var getMethod = factory.GetFunction<int>(getRequest);
            var result = getMethod.Invoke((TestInstanceClass)testCase.Instance);

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
