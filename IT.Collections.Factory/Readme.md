# IT.Collections.Factory
[![NuGet version (IT.Collections.Factory)](https://img.shields.io/nuget/v/IT.Collections.Factory.svg)](https://www.nuget.org/packages/IT.Collections.Factory)
[![NuGet pre version (IT.Collections.Factory)](https://img.shields.io/nuget/vpre/IT.Collections.Factory.svg)](https://www.nuget.org/packages/IT.Collections.Factory)

Implementation of collections factory

## Create registry

```csharp
var registry = new EnumerableFactoryRegistry();
Assert.That(registry.Count, Is.EqualTo(0));
registry.RegisterFactoriesDefault();
Assert.That(registry.Count, Is.EqualTo(86));
```

## Get ListFactory from registry and create empty list with 10 capacity

```csharp
var listFactory = registry.GetFactory<ListFactory>();
var list = listFactory.New<int>(10);
Assert.That(list.Capacity, Is.EqualTo(10));
```

## Get IReadOnlySetFactory from registry and create empty List with 10 capacity

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