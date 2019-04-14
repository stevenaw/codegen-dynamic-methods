using System;

namespace DynamicMethodGeneration.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            BeginSection("Static Members");
            TestStaticFunctions();
            TestStaticProperties();
            TestStaticFields();

            BeginSection("Instance Members");
            TestInstanceFunctions();
            TestInstanceProperties();
            TestInstanceFields();

#if !(DEBUG)
            Console.Write("Please press a key...");
            Console.Read();
#endif
        }

        private static void BeginSection(string sectionName)
        {
            var divider = new string('-', sectionName.Length);

            Console.WriteLine();
            Console.WriteLine(divider);
            Console.WriteLine(sectionName);
            Console.WriteLine(divider);
        }

        private static void TestInstanceProperties()
        {
            const int testValue = 42;
            var target = new TestClass();
            var owningType = typeof(TestClass);
            var memberInfo = owningType.GetProperty(nameof(TestClass.PropertyTest));

            var member = memberInfo.Compile<int>().WithInstance(target);

            member.Set.Invoke(testValue);
            var value = member.Get.Invoke();

            Console.WriteLine($"Return value of prop = {value}");
        }

        private static void TestInstanceFields()
        {
            const int testValue = 42;
            var target = new TestClass();
            var owningType = typeof(TestClass);
            var memberInfo = owningType.GetField(nameof(TestClass.FieldTest));

            var member = memberInfo.Compile<int>().WithInstance(target);

            member.Set.Invoke(testValue);
            var value = member.Get.Invoke();

            Console.WriteLine($"Return value of field = {value}");
        }

        private static void TestStaticProperties()
        {
            const int testValue = 42;
            var owningType = typeof(TestClass);
            var memberInfo = owningType.GetProperty(nameof(TestClass.StaticPropertyTest));

            var member = memberInfo.Compile<int>();

            member.Set.Invoke(testValue);
            var value = member.Get.Invoke();

            Console.WriteLine($"Return value of prop = {value}");
        }

        private static void TestStaticFields()
        {
            const int testValue = 42;
            var owningType = typeof(TestClass);
            var memberInfo = owningType.GetField(nameof(TestClass.StaticFieldTest));

            var member = memberInfo.Compile<int>();

            member.Set.Invoke(testValue);
            var value = member.Get.Invoke();

            Console.WriteLine($"Return value of field = {value}");
        }

        private static void TestInstanceFunctions()
        {
            var target = new TestClass();
            var owningType = typeof(TestClass);

            // Instance method /w no args and no return
            var methodNoArgsNoReturn = owningType.GetMethod(nameof(TestClass.MethodNoArgsNoReturn)).Compile();
            methodNoArgsNoReturn.WithInstance(target).Invoke();

            // Instance method /w no args and return value
            var methodNoArgsHasReturn = owningType.GetMethod(nameof(TestClass.MethodNoArgsHasReturn)).Compile<int>();
            var value = methodNoArgsHasReturn.WithInstance(target).Invoke();
            Console.WriteLine($"Return value = {value}");

            // Instance method /w args and no return
            var methodHasArgsNoReturn = owningType.GetMethod(nameof(TestClass.MethodWithArgsAndNoReturn)).Compile();
            methodHasArgsNoReturn.WithInstance(target).Invoke(2, 5);

            // Instance method /w args and return value
            var methodHasArgsHasReturn = owningType.GetMethod(nameof(TestClass.MethodWithArgsAndReturn)).Compile<int>();
            value = methodHasArgsHasReturn.WithInstance(target).Invoke(2, 5);
            Console.WriteLine($"Return value = {value}");

            // Instance method /w args and return value as DynamicInvoke
            var methodManyArgsHasReturn = owningType.GetMethod(nameof(TestClass.MethodWithManyArgsAndReturn)).Compile<int>();
            value = methodManyArgsHasReturn.WithInstance(target).Invoke(2, 3, 5, 8, 13, 21);
            Console.WriteLine($"Return value = {value}");
        }

        private static void TestStaticFunctions()
        {
            var owningType = typeof(TestClass);

            // Static method /w no args and no return
            var methodNoArgsNoReturn = owningType.GetMethod(nameof(TestClass.StaticMethodNoArgsNoReturn)).Compile();
            methodNoArgsNoReturn.Invoke();

            // Static method /w no args and return value
            var methodNoArgsHasReturn = owningType.GetMethod(nameof(TestClass.StaticMethodNoArgsHasReturn)).Compile<int>();
            var value = methodNoArgsHasReturn.Invoke();
            Console.WriteLine($"Return value = {value}");

            // Static method /w args and no return
            var methodHasArgsNoReturn = owningType.GetMethod(nameof(TestClass.StaticMethodWithArgsAndNoReturn)).Compile();
            methodHasArgsNoReturn.Invoke(2, 5);

            // Static method /w args and return value
            var methodHasArgsHasReturn = owningType.GetMethod(nameof(TestClass.StaticMethodWithArgsAndReturn)).Compile<int>();
            value = methodHasArgsHasReturn.Invoke(2, 5);
            Console.WriteLine($"Return value = {value}");
        }
    }
}
