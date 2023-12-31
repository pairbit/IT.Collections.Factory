﻿namespace IT.Collections.Factory.Tests;

using Factories;
using Internal;

public class EnumerableFactoryRegistryTest
{
    private readonly static IEnumerableFactoryRegistry Registry = new EnumerableFactoryRegistry();

    //private readonly static HashSetStringFactory _hss = new(StringComparer.Ordinal);
    //private readonly static DictionaryKeyStringFactory<int> _dks = new(StringComparer.Ordinal);

    private readonly IEnumerableFactoryRegistry _registry;
    private readonly EnumerableFactoryTester<string?> _enumerableFactoryTester;
    private readonly DictionaryFactoryTester _dictionaryFactoryTester;

    public EnumerableFactoryRegistryTest() : this(Registry) { }

    public EnumerableFactoryRegistryTest(IEnumerableFactoryRegistry registry)
    {
        TryRegisterFactories(registry);

        var factories = registry.Values.Distinct().ToArray();
        var enumerableFactories = factories.OfType<IEnumerableFactory>().ToArray();

        var listStrings = new[] { "abc", "cc", "ABC", "34", "d", "", "g", "tt", "sdfsdf", "9089" };
        //TODO: с StringComparer.Ordinal тесты падают
        var comparers = StringComparer.OrdinalIgnoreCase.ToComparers();

        _registry = registry;
        _enumerableFactoryTester = new(enumerableFactories, listStrings, comparers);
        _dictionaryFactoryTester = new(factories.OfType<IEnumerableKeyValueFactory>().ToArray());
    }

    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void TryRegisterFactoriesTest()
    {
        var registryCount = _registry.Count;

        Assert.That(TryRegisterFactories(_registry, RegistrationBehavior.None), Is.True);
        Assert.That(TryRegisterFactories(_registry, RegistrationBehavior.OverwriteExisting), Is.True);
        Assert.That(TryRegisterFactories(_registry, RegistrationBehavior.ThrowOnExisting), Is.True);

        Assert.That(registryCount, Is.EqualTo(_registry.Count));
    }

    [Test]
    public void EnumerableFactoryTest() => _enumerableFactoryTester.Test();

    [Test]
    public void DictionaryFactoryTest() => _dictionaryFactoryTester.Test();

#if !NET462
    [Test]
    public void EnumerableTupleFactoryTest()
    {
        var factories = _registry.Values.OfType<IEnumerableKeyValueTupleFactory>().Distinct().ToArray();
        
        Console.WriteLine($"{factories.Length} enumerable tuple factories");

        foreach (var factory in factories)
        {
            Console.Write($"Type '{factory.EnumerableType.FullName}' is {factory.Kind}");

            var empty = factory.Empty<string,int>();
            Assert.That(empty.Any(), Is.False);
            if (empty.TryGetCount(out var count)) Assert.That(count, Is.EqualTo(0));
            if (empty.TryGetCapacity(out var capacity)) Assert.That(capacity, Is.EqualTo(0));

            var withBuilder = factory.New<string, int>(10, tryAdd => tryAdd(("data", 10)));
            Assert.That(withBuilder.Any(), Is.True);
            if (withBuilder.TryGetCount(out count)) Assert.That(count, Is.EqualTo(1));
            if (withBuilder.TryGetCapacity(out capacity)) Assert.That(capacity, Is.EqualTo(10));
        }

        //var factory1 = _registry.GetFactory<IEnumerable<(string, int)>, (string, int)>();
    }
#endif

    [Test]
    public void ManualTest()
    {
        var listFactory = _registry.GetFactory<ListFactory>();
        ListTest(listFactory.New<int>(10), 10);

#if NETCOREAPP3_1_OR_GREATER
        var comparer = StringComparer.OrdinalIgnoreCase;
        var comparers = comparer.ToComparers();

        var immutableSetFactory = _registry.GetFactory<IImmutableSetFactory>();

        Assert.That(immutableSetFactory.GetType(), Is.EqualTo(typeof(ImmutableHashSetFactory)));
        Assert.That(immutableSetFactory.Kind.IsUnordered(), Is.True);

        ImmutableSetTest(immutableSetFactory.Empty<string?>(), order: true, unique: false);
        ImmutableSetTest(immutableSetFactory.Empty(comparers), order: true, unique: true, comparer);

        immutableSetFactory = _registry.GetFactory<ImmutableSortedSetFactory>();
        Assert.That(immutableSetFactory.Kind.IsOrdered(), Is.True);

        ImmutableSetTest(immutableSetFactory.Empty<string?>(), order: false, unique: false);
        ImmutableSetTest(immutableSetFactory.Empty(comparers), order: false, unique: true, comparer);
#endif
    }

    [Test]
    public void GenericFactoryTest()
    {
        var registryCount = _registry.Count;

        GenericEnumerableFactoryTest<int>();
        GenericEnumerableFactoryTest<int>(3);
        GenericEnumerableFactoryTest<int?>();
        GenericEnumerableFactoryTest<string>();

        GenericDictionaryFactoryTest<int, int>();
        GenericDictionaryFactoryTest<int, int?>();
        GenericDictionaryFactoryTest<string, string>();

        Assert.That(registryCount + 6, Is.EqualTo(_registry.Count));
    }

    [Test]
    public void GenericStringFactoryTest()
    {
        var hss = _registry.GetFactory<HashSetStringFactory>();
        Assert.That(hss.Kind.IsProxy(), Is.False);
        SetStringTest(hss.New(10), hss.Comparer == StringComparer.OrdinalIgnoreCase);
        Assert.That(hss.Equals(_registry.GetFactory<HashSet<string>, string>()), Is.True);
        Assert.That(hss.Equals(_registry.GetFactory<ISet<string>, string>()), Is.True);

        var dks = _registry.GetFactory<DictionaryKeyStringFactory<int>>();
        Assert.That(dks.Kind.IsProxy(), Is.False);
        DictionaryKeyStringTest(dks.New(3), dks.Comparer == StringComparer.OrdinalIgnoreCase, 5);
        Assert.That(dks.Equals(_registry.GetFactory<Dictionary<string, int>, string, int>()), Is.True);
        Assert.That(dks.Equals(_registry.GetFactory<IDictionary<string, int>, string, int>()), Is.True);
        Assert.That(dks.Equals(_registry.GetFactory<ICollection<KeyValuePair<string, int>>, KeyValuePair<string, int>>()), Is.True);

        Assert.That(_registry.GetFactory<ICollection<KeyValuePair<string, long>>, KeyValuePair<string, long>>().Kind.IsProxy(), Is.True);
    }

    private static bool TryRegisterFactories(IEnumerableFactoryRegistry registry,
        RegistrationBehavior behavior = RegistrationBehavior.ThrowOnExisting)
    {
        var comparer = StringComparer.Ordinal;
        var hss = new HashSetStringFactory(comparer);
        var dks = new DictionaryKeyStringFactory<int>(comparer);

        return registry.TryRegisterFactoriesDefault(behavior) &
#if NET6_0_OR_GREATER
               registry.TryRegisterFactory<UnorderedPriorityQueueFactory>(behavior) &
#endif
               registry.TryRegisterFactory(hss, behavior) &
               registry.TryRegisterFactory<Generic.IEnumerableFactory<HashSet<string?>, string?>>(hss, behavior) &
               registry.TryRegisterFactory<Generic.IEnumerableFactory<ISet<string?>, string?>>(hss, behavior) &
               registry.TryRegisterFactory(dks, behavior) &
               registry.TryRegisterFactory<Generic.IDictionaryFactory<Dictionary<string, int>, string, int>>(dks, behavior) &
               registry.TryRegisterFactory<Generic.IDictionaryFactory<IDictionary<string, int>, string, int>>(dks, behavior) &
               registry.TryRegisterFactory<Generic.IEnumerableFactory<ICollection<KeyValuePair<string, int>>, KeyValuePair<string, int>>>(dks, behavior);
    }

    private void SetStringTest(ISet<string?> ss, bool ignoreCase)
    {
        Assert.That(ss.Add(null), Is.True);
        Assert.That(ss.Add(null), Is.False);

        if (ignoreCase)
        {
            Assert.That(ss.Add("Test"), Is.True);
            Assert.That(ss.Add("test"), Is.False);
            Assert.That(ss.Add("tEsT"), Is.False);
            Assert.That(ss.Count, Is.EqualTo(2));
        }
        else
        {
            Assert.That(ss.Add("Test"), Is.True);
            Assert.That(ss.Add("test"), Is.True);
            Assert.That(ss.Add("tEsT"), Is.True);
            Assert.That(ss.Count, Is.EqualTo(4));
        }
    }

    private void DictionaryKeyStringTest<TValue>(IDictionary<string, TValue> dks,
        bool ignoreCase, TValue value)
    {
        if (ignoreCase)
        {
            Assert.That(dks.TryAdd("Test", value), Is.True);
            Assert.That(dks.TryAdd("test", value), Is.False);
            Assert.That(dks.TryAdd("tEsT", value), Is.False);
            Assert.That(dks.Count, Is.EqualTo(1));
        }
        else
        {
            Assert.That(dks.TryAdd("Test", value), Is.True);
            Assert.That(dks.TryAdd("test", value), Is.True);
            Assert.That(dks.TryAdd("tEsT", value), Is.True);
            Assert.That(dks.Count, Is.EqualTo(3));
        }
    }

    private void GenericEnumerableFactoryTest<T>(int capacity = 20)
    {
        var listFactory = _registry.GetFactory<List<T>, T>();

        Assert.That(listFactory.Kind.IsProxy(), Is.True);
        ListTest(listFactory.New(capacity), capacity);
    }

    private void GenericDictionaryFactoryTest<TKey, TValue>(int capacity = 20) where TKey : notnull
    {
        var dictionaryFactory = _registry.GetFactory<Dictionary<TKey, TValue>, TKey, TValue>();

        Assert.That(dictionaryFactory.Kind.IsProxy(), Is.True);
        DictionaryTest(dictionaryFactory.New(capacity));
    }

    private void ListTest<T>(List<T> list, int capacity)
    {
        Assert.That(list.Capacity, Is.EqualTo(capacity));
    }

    private void DictionaryTest<TKey, TValue>(Dictionary<TKey, TValue> dictionary) where TKey : notnull
    {
        Assert.That(dictionary.Count, Is.EqualTo(0));
    }

#if NETCOREAPP3_1_OR_GREATER
    private void ImmutableSetTest(System.Collections.Immutable.IImmutableSet<string?> immutableSet, 
        bool order, bool unique, IEqualityComparer<string?>? equalityComparer = null)
    {
        var array = new[] { "abc", "cc", "ABC", "34", "d", "" }.OrderBy(x => x).ToArray();
        if (unique) array = array.Distinct(equalityComparer).ToArray();

        int i;
        for (i = 0; i < array.Length; i++)
        {
            immutableSet = immutableSet.Add(array[i]);
        }
        i = 0;
        IEnumerable<string?> enumerable = order ? immutableSet.OrderBy(x => x) : immutableSet;

        foreach (var item in enumerable)
        {
            Assert.That(item, Is.EqualTo(array[i++]));
        }
    }
#endif
}