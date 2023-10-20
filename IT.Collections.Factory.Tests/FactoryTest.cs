using IT.Collections.Equatable;
using IT.Collections.Factory.Factories;
using System.Collections.ObjectModel;

namespace IT.Collections.Factory.Tests;

public class FactoryTest
{
    private readonly StringComparer _comparer = StringComparer.OrdinalIgnoreCase;
    private readonly int _count = 4;

    [Test]
    public void EquatableListFactoryTest()
    {
        Assert.That(ListFactory.Default, Is.Not.EqualTo(EquatableListFactory.Default));

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
        ObservableCollectionFactoryTest(ObservableCollectionFactory.Default);
    }

    public void ObservableCollectionFactoryTest(ObservableCollectionFactory factory)
    {
        Assert.That(CollectionFactory.Default, Is.Not.EqualTo(ObservableCollectionFactory.Default));

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
        Assert.That(new CollectionFactory(new ListFactory()).Equals(CollectionFactory.Default), Is.True);
        Assert.That(new CollectionFactory(new EquatableListFactory()).Equals(CollectionFactory.Default), Is.False);
        Assert.That(new ObservableCollectionFactory().Equals(ObservableCollectionFactory.Default), Is.True);
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