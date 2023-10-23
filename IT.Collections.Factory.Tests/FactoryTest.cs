using IT.Collections.Equatable;
using IT.Collections.Factory.Factories;
using System.Collections.ObjectModel;

namespace IT.Collections.Factory.Tests;

public class FactoryTest
{
    private readonly StringComparer _comparer = StringComparer.OrdinalIgnoreCase;
    private readonly int _count = 4;

    [Test]
    public void SimpleTest()
    {
        var registry = new EnumerableFactoryRegistry();
        //var registry = new ConcurrentEnumerableFactoryRegistry();
        registry.RegisterFactoriesDefault();

        var listFactory = registry.GetFactory<ListFactory>();
        var list = listFactory.Empty<int>();
        Assert.That(list.Capacity, Is.EqualTo(0));

        var arrayFactory = registry.GetFactory<ArrayFactory>();
        var array = arrayFactory.New<int>(3);
        Assert.That(array.Length, Is.EqualTo(3));

        list = listFactory.New<int>(4);
        Assert.That(list.Capacity, Is.EqualTo(4));
        Assert.That(list.Count, Is.EqualTo(0));

        Assert.That(arrayFactory.Kind.IsFixed(), Is.True);
        Assert.That(listFactory.Kind.IsFixed(), Is.False);

        var linkedListFactory = registry.GetFactory<LinkedListFactory>();

        Assert.That(linkedListFactory.Kind.IsIgnoreCapacity(), Is.True);

        var linkedList = linkedListFactory.New<int>(-1, tryAdd =>
        {
            tryAdd(1);
            tryAdd(2);
            tryAdd(3);
        });

        Assert.That(linkedList.SequenceEqual(new[] { 1, 2, 3 }), Is.True);

#if NET6_0_OR_GREATER
        var roSetFactory = registry.GetFactory<IReadOnlySetFactory>();
        Assert.That(roSetFactory.Kind.IsUnique(), Is.True);
        Assert.That(roSetFactory.Kind.IsEquatable(), Is.True);

        var roSet = roSetFactory.New(2, tryAdd =>
        {
            Assert.That(tryAdd("Test"), Is.True);
            Assert.That(tryAdd("tEsT"), Is.False);
        }, StringComparer.OrdinalIgnoreCase.ToComparers());
        Assert.That(roSet.Count, Is.EqualTo(1));
#endif

        var intStackFactory = registry.GetFactory<Stack<int>, int>();

        Assert.That(intStackFactory.Kind.IsProxy(), Is.True);
        Assert.That(intStackFactory.Kind.IsReverse(), Is.True);

        var stack = intStackFactory.New(3, tryAdd =>
        {
            tryAdd(3);
            tryAdd(2);
            tryAdd(1);
        });

        Assert.That(stack.SequenceEqual(new[] { 1, 2, 3 }), Is.True);

        IEnumerable<int> data = Enumerable.Range(5, 10);

        //EnumerableKind: None
        CheckFactory(data, registry.GetFactory<ListFactory>());

        //EnumerableKind: IgnoreCapacity
        CheckFactory(data, registry.GetFactory<LinkedListFactory>());

        //EnumerableKind: Reverse
        CheckFactory(data, registry.GetFactory<StackFactory>());

        //EnumerableKind: IgnoreCapacity, Reverse, ThreadSafe
        CheckFactory(data, registry.GetFactory<ConcurrentBagFactory>());
    }

    static void CheckFactory<T>(IEnumerable<T> data, IEnumerableFactory factory)
    {
        IEnumerable<T> newEnumerable;

        var kind = factory.Kind;

        if (kind.IsThreadSafe())
        {
            var capacity = -1;

            if (!kind.IsIgnoreCapacity())
            {
                //allocation, need use ArrayPool
                var dataArray = data.ToArray();

                capacity = dataArray.Length;

                data = dataArray;
            }

            newEnumerable = factory.New<T, IEnumerable<T>>(capacity, BuildParallel, in data);

            Assert.That(newEnumerable.OrderBy(x => x).SequenceEqual(data), Is.True);
        }

        if (kind.IsIgnoreCapacity() && !kind.IsReverse())
        {
            newEnumerable = factory.New(-1, static (TryAdd<T> tryAdd, in IEnumerable<T> data) =>
            {
                foreach (var i in data) tryAdd(i);
            }, in data);
        }
        else
        {
            //allocation, need use ArrayPool
            var dataArray = data.ToArray();

            newEnumerable = factory.New<T, T[]>(dataArray.Length,
                kind.IsReverse() ? BuildReverse : Build, in dataArray);
        }

        Assert.That(newEnumerable.SequenceEqual(data), Is.True);
    }

    static void Build<T>(TryAdd<T> tryAdd, in T[] data)
    {
        for (int i = 0; i < data.Length; i++) tryAdd(data[i]);
    }

    static void BuildReverse<T>(TryAdd<T> tryAdd, in T[] data)
    {
        for (int i = data.Length - 1; i >= 0; i--) tryAdd(data[i]);
    }

    static void BuildParallel<T>(TryAdd<T> tryAdd, in IEnumerable<T> data)
    {
        Parallel.ForEach(data, i => tryAdd(i));
    }

    [Test]
    public void EquatableListFactoryTest()
    {
        Assert.That(
            EnumerableFactoryCache<ListFactory>.Default,
            Is.Not.EqualTo(EnumerableFactoryCache<EquatableListFactory>.Default));

        ListFactory factory = new EquatableListFactory();

        var comparers = _comparer.ToComparers();

        EquatableListTest(factory.Empty(in comparers));
        EquatableListTest(factory.New(0, in comparers));
        EquatableListTest(factory.New(0, null!, in comparers));
        EquatableListTest(factory.New(0, null!, in _count, in comparers));

        EquatableListTest(factory.New(10, in comparers), count: 0, capacity: 10);
        EquatableListTest(factory.New(11, Builder, in comparers), count: 1, capacity: 11);
        EquatableListTest(factory.New(12, BuilderState, in _count, in comparers), count: _count, capacity: 12);
    }

    [Test]
    public void ObservableCollectionFactoryTest()
    {
#if !NET5_0_OR_GREATER
        ObservableCollectionFactoryTest(new MyObservableCollectionFactory());
#endif
        ObservableCollectionFactoryTest(EnumerableFactoryCache<ObservableCollectionFactory>.Default);
    }

    public void ObservableCollectionFactoryTest(ObservableCollectionFactory factory)
    {
        Assert.That(EnumerableFactoryCache<CollectionFactory>.Default, Is.Not.EqualTo(EnumerableFactoryCache<ObservableCollectionFactory>.Default));

        var type = factory.EnumerableType;

        CollectionTest(type, factory.Empty<string?>());
        CollectionTest(type, factory.New<string?>(0));
        CollectionTest(type, factory.New<string?>(0, null!));
        CollectionTest(type, factory.New<string?, int>(0, null!, in _count));

        CollectionTest(type, factory.New<string?>(10), count: 0);
        CollectionTest(type, factory.New<string?>(11, Builder), count: 1);
        CollectionTest(type, factory.New<string?, int>(12, BuilderState, in _count), count: _count);

        CollectionFactory baseFactory = factory;

        type = baseFactory.EnumerableType;
        CollectionTest(type, baseFactory.Empty<string?>());
        CollectionTest(type, baseFactory.New<string?>(0));
        CollectionTest(type, baseFactory.New<string?>(0, null!));
        CollectionTest(type, baseFactory.New<string?, int>(0, null!, in _count));

        CollectionTest(type, baseFactory.New<string?>(10), count: 0);
        CollectionTest(type, baseFactory.New<string?>(11, Builder), count: 1);
        CollectionTest(type, baseFactory.New<string?, int>(12, BuilderState, in _count), count: _count);
    }

    [Test]
    public void ComparersTest()
    {
        var ordinalIgnoreCase = StringComparer.OrdinalIgnoreCase.ToComparers();
        var ordinal = StringComparer.Ordinal.ToComparers();

        Assert.That(ordinalIgnoreCase.Equals(ordinalIgnoreCase), Is.True);
        Assert.That(ordinalIgnoreCase.Equals(ordinal), Is.False);
        Assert.That(ordinal.Equals(ordinalIgnoreCase), Is.False);

        var comparers1 = Comparers.New(StringComparer.OrdinalIgnoreCase, StringComparer.Ordinal);
        var comparers2 = Comparers.New(StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase);

        Assert.That(comparers1.Equals(comparers2), Is.False);
        Assert.That(comparers2.Equals(comparers1), Is.False);

        var comparers3 = Comparers.New(new EQ(), null);
        var comparers4 = Comparers.New(new EQ(), null);
        var comparers5 = Comparers.New(StringComparer.Ordinal, null);

        Assert.That(comparers3.Equals(comparers4), Is.True);
        Assert.That(comparers3.Equals(comparers5), Is.True);

        var ckv = Comparers.NewKeyValue(StringComparer.Ordinal);
        var ckv2 = StringComparer.Ordinal.ToComparersKeyValue();

        Assert.That(ckv.Equals(ckv2), Is.True);
    }

    public sealed class EQ : IEqualityComparer<string?>
    {
        public bool Equals(string? x, string? y)
            => StringComparer.Ordinal.Equals(x, y);

        public int GetHashCode(string? obj)
            => StringComparer.Ordinal.GetHashCode(obj);

        public override bool Equals(object? obj)
        {
            if (this == obj) return true;

            if (obj is StringComparer stringComparer && stringComparer.Equals(StringComparer.Ordinal))
            {
                return true;
            }

            return obj is EQ;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }

    [Test]
    public void EqualsTest()
    {
        Assert.That(new ListFactory().Equals(new ListFactory()), Is.True);
        Assert.That(new EquatableListFactory().Equals(new ListFactory()), Is.False);
        Assert.That(new ListFactory().Equals(new EquatableListFactory()), Is.False);
        Assert.That(new CollectionFactory(new ListFactory()).Equals(EnumerableFactoryCache<CollectionFactory>.Default), Is.True);
        Assert.That(new CollectionFactory(new EquatableListFactory()).Equals(EnumerableFactoryCache<CollectionFactory>.Default), Is.False);
        Assert.That(new ObservableCollectionFactory().Equals(EnumerableFactoryCache<ObservableCollectionFactory>.Default), Is.True);
    }

    private void EquatableListTest(List<string?> list, int count = 0, int capacity = 0)
    {
        var eqList = (EquatableList<string?>)list;
        Assert.That(ReferenceEquals(eqList.Comparer, _comparer), Is.True);
        Assert.That(eqList.Count, Is.EqualTo(count));
        Assert.That(eqList.Capacity, Is.EqualTo(capacity));
    }

    private void CollectionTest(Type typeDefinition, Collection<string?> collection, int count = 0)
    {
        var ocollection = (ObservableCollection<string?>)collection;

        Assert.That(ocollection.GetType().GetGenericTypeDefinition(), Is.EqualTo(typeDefinition));
        Assert.That(ocollection.Count, Is.EqualTo(count));
    }

    private void Builder(TryAdd<string?> tryAdd) => tryAdd("test");

    private void BuilderState(TryAdd<string?> tryAdd, in int state)
    {
        for (int i = 0; i < state; i++)
        {
            tryAdd($"test {i}");
        }
    }
}