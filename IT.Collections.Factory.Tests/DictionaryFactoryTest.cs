namespace IT.Collections.Factory.Tests;

public class DictionaryFactoryTest
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
    public void FactoryAllTest()
    {
        foreach (var pair in EnumerableFactoryRegistry.DictionaryTypes)
        {
            var type = pair.Key;
            var factory = EnumerableFactoryRegistry.TryGetDictionaryFactory(type);

            Assert.That(factory, Is.Not.Null);
            Assert.That(ReferenceEquals(factory, pair.Value), Is.True);
            Assert.That(ReferenceEquals(factory, EnumerableFactoryRegistry.GetDictionaryFactory(type)), Is.True);
        }

        var factories = EnumerableFactoryRegistry.DictionaryTypes.Values.Distinct().OrderBy(x => x.Empty<int, int>().GetType().FullName).ToArray();

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

    private void FactoryTest(IDictionaryFactory factory)
    {
        var empty = factory.Empty<int, int>();

        var type = empty.GetType();

        if (factory.Type != EnumerableType.None)
            Console.WriteLine($"Type '{type.GetGenericTypeDefinition().FullName}' is {factory.Type}");

        if (factory.Type.IsReadOnly())
        {
            Assert.Throws<NotSupportedException>(() => factory.New<int, int>(_capacity));
        }
        else
        {
            var withCapacity = factory.New<int, int>(10);

            Assert.That(withCapacity.GetType(), Is.EqualTo(type));
        }

        var array = _array;

        if ((factory.Type.IsOrdered() || factory.Type.IsUnordered()) && factory.Type.IsUnique())
        {
            array = _arraySortedUnique;
        }
        else if (factory.Type.IsOrdered() || factory.Type.IsUnordered())
        {
            array = _arraySorted;
        }
        else if (factory.Type.IsUnique())
        {
            array = _arrayUnique;
        }

        IEnumerable<KeyValuePair<int, int>> withBuilder = factory.New<int, int>(_capacity, (add, reverse) => Builder(add, reverse, factory.Type));
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

    private void Builder(TryAdd<KeyValuePair<int, int>> tryAdd, bool reverse, EnumerableType type)
    {
        Assert.That(reverse, Is.EqualTo(type.IsReverse()));

        var array = _array;
        if (reverse)
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

    private static void BuilderState(TryAdd<KeyValuePair<int, int>> tryAdd, bool reverse, in (ReadOnlyMemory<KeyValuePair<int, int>>, EnumerableType, List<int>) state)
    {
        (var memory, var type, var duplicates) = state;

        Assert.That(reverse, Is.EqualTo(type.IsReverse()));

        var span = memory.Span;
        if (reverse)
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