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

#if NET6_0_OR_GREATER
        var roSetFactory = registry.GetFactory<IReadOnlySetFactory>();
        var roSet = roSetFactory.New(2, tryAdd =>
        {
            Assert.That(tryAdd("Test"), Is.True);
            Assert.That(tryAdd("tEsT"), Is.False);
        }, StringComparer.OrdinalIgnoreCase.ToComparers());
        Assert.That(roSet.Count, Is.EqualTo(1));
#endif
        IEnumerable<int> data = Enumerable.Range(5, 10);

        CheckFactory(data, registry.GetFactory<ListFactory>());//None
        CheckFactory(data, registry.GetFactory<LinkedListFactory>());//IgnoreCapacity
        CheckFactory(data, registry.GetFactory<StackFactory>());//Reverse
        CheckFactory(data, registry.GetFactory<ConcurrentBagFactory>());//IgnoreCapacity, Reverse, ThreadSafe
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
                kind.IsReverse() ? BuildReverse : Build,
                in dataArray);
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