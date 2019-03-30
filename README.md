# Experimenting with Dynamic Code Generation

Simplified experiment using Reflection.Emit to dynamically generate functions.

## How it Works

Rather than dynamically invoking a method via Reflection, a simplified wrapper
function is generated + created.

## Demo
Simply compile + invoke. Previous compilations for a `MethodInfo` are cached internally.

```csharp
public static class TestClass
{
    public static void MethodWithArgsAndNoReturn(int a, int b)
    {
        Console.WriteLine($"Output is {a + b}");
    }
}


var owningType = typeof(TestClass);
var methodInfo = owningType.GetMethod(nameof(TestClass.MethodWithArgsAndNoReturn));
var compiledMethod = methodInfo.Compile();
methodHasArgsNoReturn.Invoke(2, 5);
```

For instance members or return types, they can be provided like so:
```csharp
public class TestClass
{
    public int MethodWithArgsAndReturn(int a, int b)
    {
        return a * b;
    }
}


var instance = new TestClass();
var methodInfo = instance.GetType().GetMethod(nameof(TestClass.MethodWithArgsAndReturn));
var compiledMethod = methodInfo.Compile<int>();
var result = methodHasArgsNoReturn.WithInstance(instance).Invoke(2, 5);
```