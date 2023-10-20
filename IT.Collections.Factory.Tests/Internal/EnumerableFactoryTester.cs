﻿namespace IT.Collections.Factory.Tests.Internal;

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

        var factories = factoryList.Distinct().OrderBy(x => x.Empty<int>().GetType().FullName).ToArray();

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
                Console.WriteLine($"Type '{factory.Empty<int>().GetType().GetGenericTypeDefinitionOrArray().FullName}' is exception");
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

        var empty = factory.Empty(in _comparers);
        Assert.That(empty.Any(), Is.False);
        if (empty.TryGetCount(out var count)) Assert.That(count, Is.EqualTo(0));
        if (empty.TryGetCapacity(out var capacity)) Assert.That(capacity, Is.EqualTo(0));
        var type = empty.GetType();
        var enumerableKind = factory.Kind;

        Console.Write($"Type '{type.GetGenericTypeDefinitionOrArray().FullName}' is {enumerableKind}");

        if (enumerableKind.IsReadOnly())
        {
            Assert.Throws<NotSupportedException>(() => factory.New(0, in _comparers));
            Assert.Throws<NotSupportedException>(() => factory.New(_capacity, in _comparers));
        }
        else
        {
            var withZero = factory.New(0, in _comparers);
            Assert.That(withZero.GetType(), Is.EqualTo(type));
            Assert.That(withZero.Any(), Is.False);
            if (withZero.TryGetCount(out count)) Assert.That(count, Is.EqualTo(0));
            if (withZero.TryGetCapacity(out capacity)) Assert.That(capacity, Is.EqualTo(0));

            var withCapacity = factory.New(_capacity, in _comparers);
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

        Assert.That(factory.New(0, null!, in _comparers).Any(), Is.False);
        Assert.That(factory.New(0, null!, 0, in _comparers).Any(), Is.False);
        Assert.Throws<ArgumentNullException>(() => factory.New(1, null!, in _comparers));
        Assert.Throws<ArgumentNullException>(() => factory.New(1, null!, 0, in _comparers));

        var data = _data;

        //TODO: Костыль IsThreadSafe
        if (enumerableKind.HasOrdered() || enumerableKind.IsThreadSafe())
        {
            data = enumerableKind.IsUnique() ? _dataSortedUnique : _dataSorted;
        }
        else if (enumerableKind.IsUnique())
        {
            data = _dataUnique;
        }

        var withBuilder = factory.New(_capacity, add => Builder(add, enumerableKind), in _comparers);
        Assert.That(withBuilder.GetType(), Is.EqualTo(type));

        //TODO: Костыль IsThreadSafe! Как проверить правильность порядка в многопоточном приложении?
        if (enumerableKind.IsUnordered() || enumerableKind.IsThreadSafe())
        {
            withBuilder = withBuilder.OrderBy(x => x).ToArray();
        }

        Assert.That(withBuilder.SequenceEqual(data), Is.True);

        var memory = new ReadOnlyMemory<T>(_data);
        var duplicates = new List<T>();
        var state = (memory, enumerableKind, duplicates, _comparers);
        var withBuilderState = factory.New<T, (ReadOnlyMemory<T>, EnumerableKind, List<T>, Comparers<T>)>
            (_capacity, BuilderState, in state, in _comparers);
        Assert.That(withBuilderState.GetType(), Is.EqualTo(type));
        
        //TODO: Костыль IsThreadSafe
        if (enumerableKind.IsUnordered() || enumerableKind.IsThreadSafe())
        {
            withBuilderState = withBuilderState.OrderBy(x => x).ToArray();
        }

        Assert.That(withBuilderState.SequenceEqual(data), Is.True);

        if (enumerableKind.IsUnique())
        {
            Assert.That(duplicates.SequenceEqual(_dataDuplicates, _comparers.EqualityComparer), Is.True);
        }
        else
        {
            Assert.That(duplicates.Count == 0, Is.True);
        }
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
            if (kind.IsReverse())
            {
                for (int i = span.Length - 1; i >= 0; i--)
                {
                    var item = span[i];
                    tasks[i] = Task.Run<(bool, T)>(() => tryAdd(item) ? (true, item) : (false, item));
                }
            }
            else
            {
                for (int i = 0; i < span.Length; i++)
                {
                    var item = span[i];
                    tasks[i] = Task.Run<(bool, T)>(() => tryAdd(item) ? (true, item) : (false, item));
                }
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
}