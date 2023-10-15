namespace IT.Collections.Factory.Tests;

public class EnumerableFactoryTest
{
    private readonly static Random _random = new();
    private int _capacity;
    private int[] _array;
    private int[] _arrayUnique;
    private int[] _arraySorted;
    private int[] _arraySortedUnique;
    private int[] _arrayDuplicates;

    [SetUp]
    public void Setup()
    {
        _capacity = 10;

        var array = new int[_capacity];

        //Check Sorted and Unique
        array[0] = 100;
        array[1] = 2;
        array[2] = 100;

        for (int i = 3; i < array.Length; i++)
        {
            array[i] = _random.Next();
        }

        _array = array;
        _arrayUnique = array.Distinct().ToArray();
        _arraySorted = array.OrderBy(x => x).ToArray();
        _arraySortedUnique = _arraySorted.Distinct().ToArray();
        _arrayDuplicates = array.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToArray();
    }

    [Test]
    public void FactoryTest()
    {
        //EnumerableFactoryRegistry.RegisterEnumerableFactory(LinkedListFactory.Default, typeof(IReadOnlyCollection<>));
        //EnumerableFactoryRegistry.RegisterEnumerableFactory(StackFactory.Default, typeof(IEnumerable<>), typeof(IReadOnlyCollection<>));
        //EnumerableFactoryRegistry.RegisterEnumerableFactory(EquatableListFactory.Default);

        foreach (var pair in StaticEnumerableFactoryRegistry.EnumerableFactories)
        {
            var type = pair.Key;
            var enumerableFactory = StaticEnumerableFactoryRegistry.TryGetEnumerableFactory(type);

            Assert.That(enumerableFactory, Is.Not.Null);
            Assert.That(ReferenceEquals(enumerableFactory, pair.Value), Is.True);
            Assert.That(ReferenceEquals(enumerableFactory, StaticEnumerableFactoryRegistry.GetEnumerableFactory(type)), Is.True);
        }

        var factories = StaticEnumerableFactoryRegistry.EnumerableFactories.Values.Distinct().OrderBy(x => x.Empty<int>().GetType().FullName).ToArray();

        Console.WriteLine($"{factories.Length} enumerable factories");

        foreach (var factory in factories)
        {
            try
            {
                FactoryTest(factory);
            }
            catch (Exception)
            {
                Console.WriteLine($"Type '{factory.Empty<int>().GetType().GetGenericTypeDefinitionOrArray().FullName}' is exception");
                throw;
            }
        }
    }

    private void FactoryTest(IEnumerableFactory factory)
    {
        var empty = factory.Empty<int>();
        Assert.That(empty.Any(), Is.False);
        if (empty.TryGetCapacity(out var capacity)) Assert.That(capacity, Is.EqualTo(0));
        var type = empty.GetType();

        if (factory.Type != EnumerableType.None)
            Console.WriteLine($"Type '{type.GetGenericTypeDefinitionOrArray().FullName}' is {factory.Type}");

        if (factory.Type.IsReadOnly())
        {
            Assert.Throws<NotSupportedException>(() => factory.New<int>(0));
            Assert.Throws<NotSupportedException>(() => factory.New<int>(_capacity));
        }
        else
        {
            var withZero = factory.New<int>(0);
            Assert.That(withZero.GetType(), Is.EqualTo(type));
            Assert.That(withZero.Any(), Is.False);
            if (withZero.TryGetCapacity(out capacity)) Assert.That(capacity, Is.EqualTo(0));

            var withCapacity = factory.New<int>(_capacity);
            //Assert.That(withCapacity.Any(), Is.False);
            Assert.That(withCapacity.GetType(), Is.EqualTo(type));
            if (withCapacity.TryGetCapacity(out capacity))
            {
                Console.WriteLine($"Type '{type.GetGenericTypeDefinitionOrArray().FullName}' has {capacity} capacity");
                Assert.That(capacity, Is.GreaterThanOrEqualTo(_capacity));
            }
        }

        Assert.That(factory.New<int>(0, null!).Any(), Is.False);
        Assert.That(factory.New<int, int>(0, null!, 0).Any(), Is.False);
        Assert.Throws<ArgumentNullException>(() => factory.New<int>(1, null!));
        Assert.Throws<ArgumentNullException>(() => factory.New<int, int>(1, null!, 0));

        var array = _array;

        if (factory.Type.IsOrdered() && factory.Type.IsUnique())
        {
            array = _arraySortedUnique;
        }
        else if (factory.Type.IsOrdered())
        {
            array = _arraySorted;
        }
        else if (factory.Type.IsUnique())
        {
            array = _arrayUnique;
        }

        var withBuilder = factory.New<int>(_capacity, add => Builder(add, factory.Type));
        Assert.That(withBuilder.GetType(), Is.EqualTo(type));
        Assert.That(withBuilder.SequenceEqual(array), Is.True);

        var memory = new ReadOnlyMemory<int>(_array);
        var duplicates = new List<int>();
        var state = (memory, factory.Type, duplicates);
        var withBuilderState = factory.New<int, (ReadOnlyMemory<int>, EnumerableType, List<int>)>(_capacity, BuilderState, in state);
        Assert.That(withBuilderState.GetType(), Is.EqualTo(type));
        Assert.That(withBuilderState.SequenceEqual(array), Is.True);

        if (factory.Type.IsUnique())
        {
            Assert.That(duplicates.SequenceEqual(_arrayDuplicates), Is.True);
        }
        else
        {
            Assert.That(duplicates.Count == 0, Is.True);
        }
    }

    private void Builder(TryAdd<int> tryAdd, EnumerableType type)
    {
        var array = _array;
        if (type.IsReverse())
        {
            for (int i = array.Length - 1; i >= 0; i--)
            {
                var item = array[i];
                if (!tryAdd(item))
                {
                    Assert.That(_arrayDuplicates.Contains(item), Is.True);
                }
            }
        }
        else
        {
            for (int i = 0; i < array.Length; i++)
            {
                var item = array[i];
                if (!tryAdd(item))
                {
                    Assert.That(_arrayDuplicates.Contains(item), Is.True);
                }
            }
        }
    }

    private static void BuilderState(TryAdd<int> tryAdd, in (ReadOnlyMemory<int>, EnumerableType, List<int>) state)
    {
        (var memory, var type, var duplicates) = state;

        var span = memory.Span;
        if (type.IsReverse())
        {
            for (int i = span.Length - 1; i >= 0; i--)
            {
                var item = span[i];
                if (!tryAdd(item)) duplicates.Add(item);
            }
        }
        else
        {
            for (int i = 0; i < span.Length; i++)
            {
                var item = span[i];
                if (!tryAdd(item)) duplicates.Add(item);
            }
        }
    }
}