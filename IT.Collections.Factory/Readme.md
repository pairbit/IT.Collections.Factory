# IT.Collections.Factory
[![NuGet version (IT.Collections.Factory)](https://img.shields.io/nuget/v/IT.Collections.Factory.svg)](https://www.nuget.org/packages/IT.Collections.Factory)
[![NuGet pre version (IT.Collections.Factory)](https://img.shields.io/nuget/vpre/IT.Collections.Factory.svg)](https://www.nuget.org/packages/IT.Collections.Factory)

Implementation of collections factory

## Register factories

```csharp
var registry = new EnumerableFactoryRegistry();
//var registry = new ConcurrentEnumerableFactoryRegistry();
registry.RegisterFactoriesDefault();
```

## New empty list

```csharp
var listFactory = registry.GetFactory<ListFactory>();
var list = listFactory.Empty<int>();
Assert.That(list.Capacity, Is.EqualTo(0));
```

## New array and new list with capacity

```csharp
var arrayFactory = registry.GetFactory<ArrayFactory>();
var array = arrayFactory.New<int>(3);
Assert.That(array.Length, Is.EqualTo(3));

list = listFactory.New<int>(4);
Assert.That(list.Capacity, Is.EqualTo(4));
Assert.That(list.Count, Is.EqualTo(0));
```

## Check EnumerableKind

```csharp
Assert.That(arrayFactory.Kind.IsFixed(), Is.True);
Assert.That(listFactory.Kind.IsFixed(), Is.False);
```

## New IReadOnlySet with comparer

```csharp
#if NET6_0_OR_GREATER
    var roSetFactory = registry.GetFactory<IReadOnlySetFactory>();
    var roSet = roSetFactory.New(2, tryAdd =>
    {
        Assert.That(tryAdd("Test"), Is.True);
        Assert.That(tryAdd("tEsT"), Is.False);
    }, StringComparer.OrdinalIgnoreCase.ToComparers());
    Assert.That(roSet.Count, Is.EqualTo(1));
#endif
```