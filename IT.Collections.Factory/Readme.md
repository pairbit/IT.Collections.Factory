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

## New collections with builder

```csharp
CheckFactory(registry.GetFactory<ListFactory>());//None
CheckFactory(registry.GetFactory<LinkedListFactory>());//IgnoreCapacity
CheckFactory(registry.GetFactory<StackFactory>());//Reverse
CheckFactory(registry.GetFactory<ConcurrentBagFactory>());//IgnoreCapacity, Reverse

static void CheckFactory(IEnumerableFactory factory)
{
    IEnumerable<int> data = Enumerable.Range(5, 15);
    IEnumerable<int> newEnumerable;

    var kind = factory.Kind;

    if (kind.IsIgnoreCapacity() && !kind.IsReverse())
    {
        newEnumerable = factory.New(-1, static (TryAdd<int> tryAdd, in IEnumerable<int> data) =>
        {
            foreach (var i in data) tryAdd(i);
        }, in data);
    }
    else
    {
        //allocation, need use ArrayPool
        var dataArray = data.ToArray();

        newEnumerable = factory.New<int, int[]>(dataArray.Length,
            kind.IsReverse() ? BuildReverse : Build, 
            in dataArray);
    }

    //test
    if (!newEnumerable.SequenceEqual(data)) throw new InvalidOperationException();
}

static void Build(TryAdd<int> tryAdd, in int[] data)
{
    for (int i = 0; i < data.Length; i++) tryAdd(data[i]);
}

static void BuildReverse(TryAdd<int> tryAdd, in int[] data)
{
    for (int i = data.Length - 1; i >= 0; i--) tryAdd(data[i]);
}
```