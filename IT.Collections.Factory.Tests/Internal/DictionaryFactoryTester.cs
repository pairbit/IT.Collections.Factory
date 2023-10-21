namespace IT.Collections.Factory.Tests.Internal;

internal class DictionaryFactoryTester
{
    private readonly static Random _random = new();
    private readonly IReadOnlyList<IEnumerableKeyValueFactory> _factoryList;
    private readonly Comparers<int, int> _comparers;

    private int _capacity;
    private KeyValuePair<int, int>[] _array;
    private int[] _keys;
    private int[] _keysUnique;
    private int[] _keysSorted;
    private int[] _keysSortedUnique;
    private int[] _keysDuplicates;

    public DictionaryFactoryTester(IReadOnlyList<IEnumerableKeyValueFactory> factoryList)
    {
        _factoryList = factoryList;
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
        _keys = array.Select(x => x.Key).ToArray();
        _keysUnique = dic.Keys.ToArray();
        _keysSorted = _keys.OrderBy(x => x).ToArray();
        _keysSortedUnique = _keysUnique.OrderBy(x => x).ToArray();
        _keysDuplicates = array.GroupBy(x => x.Key).Where(x => x.Count() > 1).Select(x => x.Key).ToArray();
    }

    public void Test()
    {
        var factoryList = _factoryList;

        var factories = factoryList.Distinct().OrderBy(x => x.Empty<int, int>().GetType().FullName).ToArray();

        Console.WriteLine($"{factories.Length} dictionary factories");

        foreach (var factory in factories)
        {
            try
            {
                Test(factory);
            }
            catch (Exception)
            {
                Console.WriteLine();
                Console.WriteLine($"Type '{factory.Empty<int, int>().GetType().GetGenericTypeDefinition().FullName}' is exception in Factory '{factory.GetType().FullName}'");
                throw;
            }
            finally
            {
                Console.WriteLine();
            }
        }
    }

    public void Test(IEnumerableKeyValueFactory factory)
    {
        var factoryType = factory.GetType();
        var newFactory = Activator.CreateInstance(factoryType);
        Assert.That(newFactory, Is.Not.Null);
        Assert.That(newFactory == factory, Is.False);
        Assert.That(newFactory.Equals(factory), Is.True);

        var kind = factory.Kind;
        Console.Write($"Type '{factory.EnumerableType.FullName}' is {kind}");

        var empty = factory.Empty<int, int>();
        CheckEmpty(empty);
        var type = empty.GetType();
        
        if (kind.IsReadOnly())
        {
            Assert.Throws<NotSupportedException>(() => factory.New<int, int>(0));
            Assert.Throws<NotSupportedException>(() => factory.New<int, int>(_capacity));
        }
        else
        {
            if (kind.IsIgnoreCapacity())
            {
                CheckEmpty(factory.New<int, int>(-100));
            }
            else
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => factory.New<int, int>(-100));
            }

            var withZero = factory.New<int, int>(0);
            Assert.That(withZero.GetType(), Is.EqualTo(type));
            CheckEmpty(withZero);

            var withCapacity = factory.New<int, int>(_capacity);
            Assert.That(withCapacity.GetType(), Is.EqualTo(type));
            if (kind.IsFixed())
            {
                Assert.That(withCapacity.Any(), Is.True);
                if (withCapacity.TryGetCount(out var count))
                {
                    Console.Write($", Count {count}");
                    Assert.That(count, Is.EqualTo(_capacity));
                }
            }
            else
            {
                Assert.That(withCapacity.Any(), Is.False);
                if (withCapacity.TryGetCapacity(out var capacity))
                {
                    Console.Write($", Capacity {capacity}");
                    Assert.That(capacity, Is.GreaterThanOrEqualTo(_capacity));
                }
            }
        }

        CheckEmpty(factory.New<int, int>(0, null!));
        CheckEmpty(factory.New<int, int>(0, tryAdd => tryAdd(default)));
        
        Assert.Throws<ArgumentNullException>(() => factory.New<int, int>(-99, null!));
        Assert.Throws<ArgumentNullException>(() => factory.New<int, int>(1, null!));

        var keys = _keys;
        if (kind.HasOrdered()) keys = kind.IsUnique() ? _keysSortedUnique : _keysSorted;
        else if (kind.IsUnique()) keys = _keysUnique;

        IEnumerable<KeyValuePair<int, int>> withBuilder;

        if (kind.IsIgnoreCapacity())
        {
            withBuilder = factory.New<int, int>(-99, add => Builder(add, kind));
        }
        else
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => factory.New<int, int>(-99, add => Builder(add, kind)));

            withBuilder = factory.New<int, int>(_capacity, add => Builder(add, kind));
        }

        Assert.That(withBuilder.GetType(), Is.EqualTo(type));

        var newKeys = withBuilder.Select(x => x.Key).ToArray();

        if (kind.IsUnordered())
        {
            newKeys = newKeys.OrderBy(x => x).ToArray();
        }

        Assert.That(newKeys.SequenceEqual(keys), Is.True);

        CheckEmpty(factory.New<int, int, int>(0, null!, 0));
        CheckEmpty(factory.New<int, int, int>(0, BuilderStateTest, 0));
        
        Assert.Throws<ArgumentNullException>(() => factory.New<int, int, int>(-99, null!, 0));
        Assert.Throws<ArgumentNullException>(() => factory.New<int, int, int>(1, null!, 0));

        var memory = new ReadOnlyMemory<KeyValuePair<int, int>>(_array);
        var duplicates = new List<int>();
        var state = (memory, kind, duplicates);
        IEnumerable<KeyValuePair<int, int>> withBuilderState;

        if (kind.IsIgnoreCapacity())
        {
            withBuilderState = factory.New(-99, BuilderState, in state, in _comparers);
        }
        else
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => factory.New(-99, BuilderState, in state, in _comparers));
            withBuilderState = factory.New(_capacity, BuilderState, in state, in _comparers);
        }

        Assert.That(withBuilderState.GetType(), Is.EqualTo(type));

        newKeys = withBuilderState.Select(x => x.Key).ToArray();

        if (kind.IsUnordered())
        {
            newKeys = newKeys.OrderBy(x => x).ToArray();
        }
        Assert.That(newKeys.SequenceEqual(keys), Is.True);

        if (kind.IsUnique())
        {
            Assert.That(duplicates.SequenceEqual(_keysDuplicates), Is.True);
        }
        else
        {
            Assert.That(duplicates.Count == 0, Is.True);
        }
    }

    private void Builder(TryAdd<KeyValuePair<int, int>> tryAdd, EnumerableKind kind)
    {
        var array = _array;
        if (kind.IsThreadSafe())
        {
            var tasks = new Task<(bool, int)>[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                var item = array[i];
                tasks[i] = Task.Run<(bool, int)>(() => tryAdd(item) ? (true, item.Key) : (false, item.Key));
            }

            Task.WaitAll(tasks);

            for (int i = 0; i < tasks.Length; i++)
            {
                var task = tasks[i];
                (var added, var key) = task.Result;
                if (!added) Assert.That(_keysDuplicates.Contains(key), Is.True);
            }
        }
        else
        {
            if (kind.IsReverse())
            {
                for (int i = array.Length - 1; i >= 0; i--)
                {
                    var item = array[i];
                    if (!tryAdd(item))
                    {
                        Assert.That(_keysDuplicates.Contains(item.Key), Is.True);
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
                        Assert.That(_keysDuplicates.Contains(item.Key), Is.True);
                    }
                }
            }
        }
    }

    private static void BuilderState(TryAdd<KeyValuePair<int, int>> tryAdd, in (ReadOnlyMemory<KeyValuePair<int, int>>, EnumerableKind, List<int>) state)
    {
        (var memory, var kind, var duplicates) = state;

        var span = memory.Span;

        if (kind.IsThreadSafe())
        {
            var tasks = new Task<(bool, int)>[span.Length];
            if (kind.IsReverse())
            {
                for (int i = span.Length - 1; i >= 0; i--)
                {
                    var item = span[i];
                    tasks[i] = Task.Run<(bool, int)>(() => tryAdd(item) ? (true, item.Key) : (false, item.Key));
                }
            }
            else
            {
                for (int i = 0; i < span.Length; i++)
                {
                    var item = span[i];
                    tasks[i] = Task.Run<(bool, int)>(() => tryAdd(item) ? (true, item.Key) : (false, item.Key));
                }
            }
            Task.WaitAll(tasks);

            for (int i = 0; i < tasks.Length; i++)
            {
                var task = tasks[i];
                (var added, var key) = task.Result;
                if (!added) duplicates.Add(key);
            }
        }
        else
        {
            if (kind.IsReverse())
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

    private static void BuilderStateTest(TryAdd<KeyValuePair<int, int>> tryAdd, in int data)
    {
        tryAdd(default);
    }

    private static void CheckEmpty(IEnumerable<KeyValuePair<int, int>> empty)
    {
        Assert.That(empty.Any(), Is.False);
        if (empty.TryGetCount(out var count)) Assert.That(count, Is.EqualTo(0));
        if (empty.TryGetCapacity(out var capacity)) Assert.That(capacity, Is.EqualTo(0));
    }
}