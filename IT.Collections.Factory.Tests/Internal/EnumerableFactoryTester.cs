namespace IT.Collections.Factory.Tests.Internal;

internal class EnumerableFactoryTester<T>
{
    private readonly IReadOnlyList<IEnumerableFactory> _factoryList;
    private readonly Comparers<T> _comparers;

    private int _capacity;
    private T[] _data;
    private T[] _dataUnique;
    private T[] _dataSorted;
    private T[] _dataSortedUnique;
    private T[] _dataDuplicates;

    public EnumerableFactoryTester(
        IReadOnlyList<IEnumerableFactory> factoryList,
        T[] data,
        Comparers<T> comparers = default)
    {
        _factoryList = factoryList;
        _comparers = comparers;

        var equalityComparer = comparers.EqualityComparer;
        var comparer = comparers.Comparer;

        _capacity = data.Length;
        _data = data;
        _dataUnique = data.Distinct(equalityComparer).ToArray();
        _dataSorted = data.OrderBy(x => x, comparer).ToArray();
        _dataSortedUnique = _dataSorted.Distinct(equalityComparer).ToArray();
        _dataDuplicates = data.GroupBy(x => x, equalityComparer).Where(x => x.Count() > 1).Select(x => x.Key).ToArray();
    }

    public void Test()
    {
        var factoryList = _factoryList;

        var factories = factoryList.Distinct().OrderBy(x => x.EnumerableType.FullName).ToArray();

        Console.WriteLine($"{factories.Length} enumerable factories");

        foreach (var factory in factories)
        {
            try
            {
                Test(factory);
            }
            catch (Exception)
            {
                Console.WriteLine();
                Console.WriteLine($"Type '{factory.EnumerableType.FullName}' is exception");
                throw;
            }
            finally
            {
                Console.WriteLine();
            }
        }
    }

    public void Test(IEnumerableFactory factory)
    {
        var factoryType = factory.GetType();
        var newFactory = Activator.CreateInstance(factoryType);
        Assert.That(newFactory, Is.Not.Null);
        Assert.That(newFactory == factory, Is.False);
        Assert.That(newFactory.Equals(factory), Is.True);

        var kind = factory.Kind;
        var enumerableType = factory.EnumerableType;
        Console.Write($"Type '{enumerableType.FullName}' is {kind}");

        var empty = factory.Empty(in _comparers);
        CheckEmpty(empty);
        var type = empty.GetType();

        NewWithCapacity(kind, type, capacity => factory.New(capacity, in _comparers), _capacity);

        var data = GetData(kind);

        var dataBuild = NewWithBuilder(kind, type,
            (capacity, withBuilder) =>
            factory.New(capacity, withBuilder ? add => Builder(add, kind) : null!, in _comparers),
            _capacity);

        Assert.That(dataBuild.SequenceEqual(data), Is.True);

        Assert.That(NewWithBuilderState(factory, type).SequenceEqual(data), Is.True);
    }

    private static void NewWithCapacity(
        EnumerableKind kind, Type type,
        Func<int, IEnumerable<T>> funcNew, int capacity)
    {
        if (kind.IsReadOnly())
        {
            Assert.Throws<NotSupportedException>(() => funcNew(-100));
            Assert.Throws<NotSupportedException>(() => funcNew(0));
            Assert.Throws<NotSupportedException>(() => funcNew(capacity));

            return;
        }

        if (kind.IsIgnoreCapacity())
        {
            CheckEmpty(funcNew(-100));
        }
        else
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => funcNew(-99)).CheckCapacity();
        }

        var withZero = funcNew(0);
        Assert.That(withZero.GetType(), Is.EqualTo(type));
        CheckEmpty(withZero);

        var withCapacity = funcNew(capacity);
        Assert.That(withCapacity.GetType(), Is.EqualTo(type));

        if (kind.IsFixed())
        {
            Assert.That(withCapacity.Any(), Is.True);
            if (withCapacity.TryGetCount(out var count))
            {
                Console.Write($", Count {count}");
                Assert.That(count, Is.EqualTo(capacity));
            }
        }
        else
        {
            Assert.That(withCapacity.Any(), Is.False);
            if (withCapacity.TryGetCapacity(out var c))
            {
                Console.Write($", Capacity {c}");
                Assert.That(c, Is.GreaterThanOrEqualTo(capacity));
            }
        }
    }

    private IEnumerable<T> NewWithBuilderState(IEnumerableFactory factory, Type type)
    {
        var kind = factory.Kind;

        var memory = new ReadOnlyMemory<T>(_data);
        var duplicates = new List<T>();
        var state = (memory, kind, duplicates, _comparers);

        var data = NewWithBuilder(kind, type,
            (capacity, withBuilder) =>
            factory.New(capacity, withBuilder ? BuilderState : null!, in state, in _comparers),
            _capacity);

        if (kind.IsUnique())
        {
            Assert.That(duplicates.SequenceEqual(_dataDuplicates, _comparers.EqualityComparer), Is.True);
        }
        else
        {
            Assert.That(duplicates.Count == 0, Is.True);
        }

        return data;
    }

    private static IEnumerable<T> NewWithBuilder(EnumerableKind kind, Type type,
        Func<int, bool, IEnumerable<T>> funcNew, int capacity)
    {
        CheckEmpty(funcNew(0, false));
        CheckEmpty(funcNew(0, true));

        //builder is null
        Assert.Throws<ArgumentNullException>(() => funcNew(-100, false)).CheckBuilder();
        Assert.Throws<ArgumentNullException>(() => funcNew(100, false)).CheckBuilder();

        IEnumerable<T> data;
        if (kind.IsIgnoreCapacity())
        {
            data = funcNew(-100, true);
        }
        else
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => funcNew(-99, true)).CheckCapacity();
            data = funcNew(capacity, true);
        }

        Assert.That(data.GetType(), Is.EqualTo(type));

        //TODO: Костыль IsThreadSafe! Как проверить правильность порядка в многопоточном приложении?
        if (kind.IsUnordered() || kind.IsThreadSafe())
        {
            data = data.OrderBy(x => x).ToArray();
        }

        return data;
    }

    private void Builder(TryAdd<T> tryAdd, EnumerableKind kind)
    {
        var array = _data;

        if (kind.IsThreadSafe())
        {
            var tasks = new Task<(bool, T)>[array.Length];
            if (kind.IsReverse())
            {
                for (int i = array.Length - 1; i >= 0; i--)
                {
                    var item = array[i];
                    tasks[i] = Task.Run<(bool, T)>(() => tryAdd(item) ? (true, item) : (false, item));
                }
            }
            else
            {
                for (int i = 0; i < array.Length; i++)
                {
                    var item = array[i];
                    tasks[i] = Task.Run<(bool, T)>(() => tryAdd(item) ? (true, item) : (false, item));
                }
            }
            Task.WaitAll(tasks);

            for (int i = 0; i < tasks.Length; i++)
            {
                var task = tasks[i];
                (var added, var item) = task.Result;
                if (!added) Assert.That(_dataDuplicates.Contains(item, _comparers.EqualityComparer), Is.True);
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
                        Assert.That(_dataDuplicates.Contains(item, _comparers.EqualityComparer), Is.True);
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
                        Assert.That(_dataDuplicates.Contains(item, _comparers.EqualityComparer), Is.True);
                    }
                }
            }
        }
    }

    private static void BuilderState(TryAdd<T> tryAdd, in (ReadOnlyMemory<T>, EnumerableKind, List<T>, Comparers<T>) state)
    {
        (var memory, var kind, var duplicates, var comparers) = state;
        var span = memory.Span;

        if (kind.IsThreadSafe())
        {
            var tasks = new Task<(bool, T)>[span.Length];
            for (int i = 0; i < span.Length; i++)
            {
                var item = span[i];
                tasks[i] = Task.Run<(bool, T)>(() => tryAdd(item) ? (true, item) : (false, item));
            }

            Task.WaitAll(tasks);

            for (int i = 0; i < tasks.Length; i++)
            {
                var task = tasks[i];
                (var added, var item) = task.Result;
                if (!added) duplicates.Add(item);
            }
        }
        else
        {
            if (kind.IsReverse())
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

    private static void CheckEmpty(IEnumerable<T> empty)
    {
        Assert.That(empty.Any(), Is.False);
        if (empty.TryGetCount(out var count)) Assert.That(count, Is.EqualTo(0));
        if (empty.TryGetCapacity(out var capacity)) Assert.That(capacity, Is.EqualTo(0));
    }

    private T[] GetData(EnumerableKind kind)
    {
        //TODO: Костыль IsThreadSafe
        if (kind.HasOrdered() || kind.IsThreadSafe())
        {
            return kind.IsUnique() ? _dataSortedUnique : _dataSorted;
        }
        else if (kind.IsUnique())
        {
            return _dataUnique;
        }

        return _data;
    }
}