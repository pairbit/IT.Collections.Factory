namespace IT.Collections.Factory.Tests;

public class StaticDictionaryFactoryTest
{
    private readonly static Random _random = new();
    private int _capacity;
    private KeyValuePair<int, int>[] _array;
    private KeyValuePair<int, int>[] _arrayUnique;
    private KeyValuePair<int, int>[] _arraySorted;
    private KeyValuePair<int, int>[] _arraySortedUnique;
    private int[] _arrayKeyDuplicates;

    [SetUp]
    public void Setup()
    {
        _capacity = 10;

        var array = new KeyValuePair<int, int>[_capacity];

        array[0] = new KeyValuePair<int, int>(100, 0);
        array[1] = new KeyValuePair<int, int>(2, 1);
        array[2] = new KeyValuePair<int, int>(100, 2);

        for (int i = 3; i < _capacity; i++)
        {
            array[i] = new KeyValuePair<int, int>(_random.Next(), i);
        }

        var dic = new Dictionary<int, int>(array.Length);

        for (int i = 0; i < array.Length; i++)
        {
            var item = array[i];
            if (!dic.ContainsKey(item.Key)) dic.Add(item.Key, item.Value);
        }

        _array = array;
        _arrayUnique = dic.ToArray();
        _arraySorted = array.OrderBy(x => x.Key).ToArray();
        _arraySortedUnique = dic.OrderBy(x => x.Key).ToArray();
        _arrayKeyDuplicates = array.GroupBy(x => x.Key).Where(x => x.Count() > 1).Select(x => x.Key).ToArray();
    }

    [Test]
    public void FactoryTest()
    {
        foreach (var pair in StaticEnumerableFactoryRegistry.DictionaryFactories)
        {
            var type = pair.Key;
            var factory = StaticEnumerableFactoryRegistry.TryGetDictionaryFactory(type);

            Assert.That(factory, Is.Not.Null);
            Assert.That(ReferenceEquals(factory, pair.Value), Is.True);
            Assert.That(ReferenceEquals(factory, StaticEnumerableFactoryRegistry.GetDictionaryFactory(type)), Is.True);
        }

        var factories = StaticEnumerableFactoryRegistry.DictionaryFactories.Values.Distinct().OrderBy(x => x.Empty<int, int>().GetType().FullName).ToArray();

        Console.WriteLine($"{factories.Length} dictionary factories");

        foreach (var factory in factories)
        {
            try
            {
                FactoryTest(factory);
            }
            catch (Exception)
            {
                Console.WriteLine($"Type '{factory.Empty<int, int>().GetType().GetGenericTypeDefinition().FullName}' is exception in Factory '{factory.GetType().FullName}'");
                throw;
            }
        }
    }

    private void FactoryTest(IEnumerableKeyValueFactory factory)
    {
        var empty = factory.Empty<int, int>();
        Assert.That(empty.Any(), Is.False);
        if (empty.TryGetCapacity(out var capacity)) Assert.That(capacity, Is.EqualTo(0));
        var type = empty.GetType();

        if (factory.Type != EnumerableType.None)
            Console.WriteLine($"Type '{type.GetGenericTypeDefinition().FullName}' is {factory.Type}");

        if (factory.Type.IsReadOnly())
        {
            Assert.Throws<NotSupportedException>(() => factory.New<int, int>(0));
            Assert.Throws<NotSupportedException>(() => factory.New<int, int>(_capacity));
        }
        else
        {
            var withZero = factory.New<int, int>(0);
            Assert.That(withZero.GetType(), Is.EqualTo(type));
            Assert.That(withZero.Any(), Is.False);
            if (withZero.TryGetCapacity(out capacity)) Assert.That(capacity, Is.EqualTo(0));

            var withCapacity = factory.New<int, int>(_capacity);
            //Assert.That(withCapacity.Any(), Is.False);
            Assert.That(withCapacity.GetType(), Is.EqualTo(type));
            if (withCapacity.TryGetCapacity(out capacity))
            {
                Console.WriteLine($"Type '{type.GetGenericTypeDefinition().FullName}' has {capacity} capacity");
                Assert.That(capacity, Is.GreaterThanOrEqualTo(_capacity));
            }
        }

        Assert.That(factory.New<int, int>(0, null!).Any(), Is.False);
        Assert.That(factory.New<int, int, int>(0, null!, 0).Any(), Is.False);
        Assert.Throws<ArgumentNullException>(() => factory.New<int, int>(1, null!));
        Assert.Throws<ArgumentNullException>(() => factory.New<int, int, int>(1, null!, 0));

        var array = _array;

        if (factory.Type.HasOrdered() && factory.Type.IsUnique())
        {
            array = _arraySortedUnique;
        }
        else if (factory.Type.HasOrdered())
        {
            array = _arraySorted;
        }
        else if (factory.Type.IsUnique())
        {
            array = _arrayUnique;
        }

        IEnumerable<KeyValuePair<int, int>> withBuilder = factory.New<int, int>(_capacity, add => Builder(add, factory.Type));
        Assert.That(withBuilder.GetType(), Is.EqualTo(type));

        if (factory.Type.IsUnordered())
        {
            withBuilder = withBuilder.OrderBy(x => x.Key).ToArray();
        }

        Assert.That(withBuilder.SequenceEqual(array), Is.True);

        var memory = new ReadOnlyMemory<KeyValuePair<int, int>>(_array);
        var duplicates = new List<int>();
        var state = (memory, factory.Type, duplicates);
        IEnumerable<KeyValuePair<int, int>> withBuilderState = 
            factory.New<int, int, (ReadOnlyMemory<KeyValuePair<int, int>>, EnumerableType, List<int>)>(_capacity, BuilderState, in state);
        Assert.That(withBuilderState.GetType(), Is.EqualTo(type));

        if (factory.Type.IsUnordered())
        {
            withBuilderState = withBuilderState.OrderBy(x => x.Key).ToArray();
        }
        Assert.That(withBuilderState.SequenceEqual(array), Is.True);

        if (factory.Type.IsUnique())
        {
            Assert.That(duplicates.SequenceEqual(_arrayKeyDuplicates), Is.True);
        }
        else
        {
            Assert.That(duplicates.Count == 0, Is.True);
        }
    }

    private void Builder(TryAdd<KeyValuePair<int, int>> tryAdd, EnumerableType type)
    {
        var array = _array;
        if (type.IsReverse())
        {
            for (int i = array.Length - 1; i >= 0; i--)
            {
                var item = array[i];
                if (!tryAdd(item))
                {
                    Assert.That(_arrayKeyDuplicates.Contains(item.Key), Is.True);
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
                    Assert.That(_arrayKeyDuplicates.Contains(item.Key), Is.True);
                }
            }
        }
    }

    private static void BuilderState(TryAdd<KeyValuePair<int, int>> tryAdd, in (ReadOnlyMemory<KeyValuePair<int, int>>, EnumerableType, List<int>) state)
    {
        (var memory, var type, var duplicates) = state;

        var span = memory.Span;
        if (type.IsReverse())
        {
            for (int i = span.Length - 1; i >= 0; i--)
            {
                var item = span[i];
                if (!tryAdd(item)) duplicates.Add(item.Key);
            }
        }
        else
        {
            for (int i = 0; i < span.Length; i++)
            {
                var item = span[i];
                if (!tryAdd(item)) duplicates.Add(item.Key);
            }
        }
    }
}