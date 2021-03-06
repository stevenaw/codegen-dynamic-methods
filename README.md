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

### Properties and Fields
For properties and fields, the same conventions apply. As properties and fields can be both set and get,
they must be accessed through `Get` and `Set`.

```csharp
public class TestClass
{
    public int PropertyTest { get; set; }
}

var instance = new TestClass();
var memberInfo = instance.GetType().GetProperty(nameof(TestClass.PropertyTest));

var member = memberInfo.Compile<int>().WithInstance(instance);

member.Set.Invoke(42);
var value = member.Get.Invoke(42);
```

Alternately, `CompileSetter` and `CompilerGetter` methods are also directly available.

```csharp
public class TestClass
{
    public int PropertyTest { get; set; }
}

var instance = new TestClass();
var memberInfo = instance.GetType().GetProperty(nameof(TestClass.PropertyTest));

var setter = memberInfo.CompileSetter<int>().WithInstance(instance);
var getter = memberInfo.CompileGetter<int>().WithInstance(instance);

setter.Invoke(42);
var value = getter.Invoke(42);
```

## Benchmarks
A few benchmarks as run from my machine.

|                          Method |      Mean |     Error |    StdDev |    Median |
|-------------------------------- |----------:|----------:|----------:|----------:|
|       Codegen_GenerateAndInvoke | 134.18 ns | 2.7031 ns | 6.0459 ns | 130.87 ns |
|              Codegen_BareInvoke |  27.57 ns | 1.2707 ns | 3.7467 ns |  28.66 ns |
| Codegen_BareInvoke_WithInstance |  37.81 ns | 0.6034 ns | 0.5644 ns |  37.62 ns |
|           Reflection_BareInvoke | 250.10 ns | 1.5043 ns | 1.4071 ns | 250.50 ns |