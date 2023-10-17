using IT.Collections.Equatable;
using IT.Collections.Factory.Factories;
using System.Collections.ObjectModel;
using System.Drawing;

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
        Assert.That(CollectionFactory.Default, Is.Not.EqualTo(ObservableCollectionFactory.Default));

        CollectionFactory factory = new ObservableCollectionFactory();

        CollectionTest(factory.Empty<string?>());
        CollectionTest(factory.New<string?>(0));
        CollectionTest(factory.New<string?>(0, null!));
        CollectionTest(factory.New<string?, int>(0, null!, in _count));

        CollectionTest(factory.New<string?>(10), count: 0);
        CollectionTest(factory.New<string?>(11, Builder), count: 1);
        CollectionTest(factory.New<string?, int>(12, BuilderState, in _count), count: _count);
    }

    private void EquatableListTest(List<string?> list, int count = 0, int capacity = 0)
    {
        var eqList = (EquatableList<string?>)list;
        Assert.That(ReferenceEquals(eqList.Comparer, _comparer), Is.True);
        Assert.That(eqList.Count, Is.EqualTo(count));
        Assert.That(eqList.Capacity, Is.EqualTo(capacity));
    }

    private void CollectionTest(Collection<string?> collection, int count = 0)
    {
        var ocollection = (ObservableCollection<string?>)collection;

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