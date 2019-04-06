# Experimenting with Dynamic Code Generation

Simplified experiment using Reflection.Emit to dynamically generate functions.

## How it Works

Rather than dynamically invoking a method via Reflection, a simplified wrapper
function is generated + created.

## Examples
Simply compile + invoke. Previous compilations for a `MethodInfo` are cached internally.

### Static Members
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

### Return Types
For members with return types, they can be provided using the generic version of `Compile<T>()`:

```csharp
public static class TestClass
{
    public static int MethodWithArgsAndReturn(int a, int b)
    {
        return a + b;
    }
}


var owningType = typeof(TestClass);
var methodInfo = owningType.GetMethod(nameof(TestClass.MethodWithArgsAndReturn));
var compiledMethod = methodInfo.Compile<int>();
var result = methodHasArgsNoReturn.Invoke(2, 5);
```

### Instance Members
For instance members, the `WithInstance<T>(TInstance)` function will associate the instance of the class with
the non-static method.

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