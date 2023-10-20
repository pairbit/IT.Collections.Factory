namespace IT.Collections.Factory.Tests.Internal;

internal class DictionaryFactoryTester
{
    private readonly static Random _random = new();
    private readonly IReadOnlyList<IEnumerableKeyValueFactory> _factoryList;
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

        var empty = factory.Empty<int, int>();
        Assert.That(empty.Any(), Is.False);
        if (empty.TryGetCount(out var count)) Assert.That(count, Is.EqualTo(0));
        if (empty.TryGetCapacity(out var capacity)) Assert.That(capacity, Is.EqualTo(0));
        var type = empty.GetType();
        var enumerableKind = factory.Kind;

        Console.Write($"Type '{type.GetGenericTypeDefinitionOrArray().FullName}' is {enumerableKind}");

        if (enumerableKind.IsReadOnly())
        {
            Assert.Throws<NotSupportedException>(() => factory.New<int, int>(0));
            Assert.Throws<NotSupportedException>(() => factory.New<int, int>(_capacity));
        }
        else
        {
            var withZero = factory.New<int, int>(0);
            Assert.That(withZero.GetType(), Is.EqualTo(type));
            Assert.That(withZero.Any(), Is.False);
            if (withZero.TryGetCount(out count)) Assert.That(count, Is.EqualTo(0));
            if (withZero.TryGetCapacity(out capacity)) Assert.That(capacity, Is.EqualTo(0));

            var withCapacity = factory.New<int, int>(_capacity);
            Assert.That(withCapacity.GetType(), Is.EqualTo(type));
            if (enumerableKind.IsFixed())
            {
                Assert.That(withCapacity.Any(), Is.True);
                if (withCapacity.TryGetCount(out count))
                {
                    Console.Write($", Count {count}");
                    Assert.That(count, Is.EqualTo(_capacity));
                }
            }
            else
            {
                Assert.That(withCapacity.Any(), Is.False);
                if (withCapacity.TryGetCapacity(out capacity))
                {
                    Console.Write($", Capacity {capacity}");
                    Assert.That(capacity, Is.GreaterThanOrEqualTo(_capacity));
                }
            }
        }

        Assert.That(factory.New<int, int>(0, null!).Any(), Is.False);
        Assert.That(factory.New<int, int, int>(0, null!, 0).Any(), Is.False);
        Assert.Throws<ArgumentNullException>(() => factory.New<int, int>(1, null!));
        Assert.Throws<ArgumentNullException>(() => factory.New<int, int, int>(1, null!, 0));

        var keys = _keys;

        if (enumerableKind.HasOrdered())
        {
            keys = enumerableKind.IsUnique() ? _keysSortedUnique : _keysSorted;
        }
        else if (enumerableKind.IsUnique())
        {
            keys = _keysUnique;
        }

        IEnumerable<KeyValuePair<int, int>> withBuilder = factory.New<int, int>(_capacity, add => Builder(add, factory.Kind));
        Assert.That(withBuilder.GetType(), Is.EqualTo(type));

        var newKeys = withBuilder.Select(x => x.Key).ToArray();

        if (enumerableKind.IsUnordered())
        {
            newKeys = newKeys.OrderBy(x => x).ToArray();
        }

        Assert.That(newKeys.SequenceEqual(keys), Is.True);

        var memory = new ReadOnlyMemory<KeyValuePair<int, int>>(_array);
        var duplicates = new List<int>();
        var state = (memory, enumerableKind, duplicates);
        IEnumerable<KeyValuePair<int, int>> withBuilderState =
            factory.New<int, int, (ReadOnlyMemory<KeyValuePair<int, int>>, EnumerableKind, List<int>)>(_capacity, BuilderState, in state);
        Assert.That(withBuilderState.GetType(), Is.EqualTo(type));

        newKeys = withBuilderState.Select(x => x.Key).ToArray();

        if (enumerableKind.IsUnordered())
        {
            newKeys = newKeys.OrderBy(x => x).ToArray();
        }
        Assert.That(newKeys.SequenceEqual(keys), Is.True);

        if (enumerableKind.IsUnique())
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
}