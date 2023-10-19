namespace IT.Collections.Factory.Tests;

using Factories;
using Internal;

public class EnumerableFactoryRegistryTest
{
    private readonly static IEnumerableFactoryRegistry Registry = 
        new EnumerableFactoryRegistry();

    private readonly IEnumerableFactoryRegistry _registry;
    private readonly EnumerableFactoryTester<string?> _enumerableFactoryTester;
    private readonly DictionaryFactoryTester _dictionaryFactoryTester;

    public EnumerableFactoryRegistryTest() : this(Registry) { }

    public EnumerableFactoryRegistryTest(IEnumerableFactoryRegistry registry)
    {
        registry.RegisterFactoriesDefaultAndInterfaces();
        var hssFactory = new HashSetStringFactory(StringComparer.OrdinalIgnoreCase);
        registry.RegisterFactory(hssFactory);
        //registry.RegisterFactory<Generic.IEnumerableFactory<HashSet<string?>, string?>>(hssFactory, RegistrationBehavior.None);
        registry.RegisterFactory<Generic.IEnumerableFactory<ISet<string?>, string?>>(hssFactory, RegistrationBehavior.None);

        var factories = registry.Values.Distinct().ToArray();
        var enumerableFactories = factories.OfType<IEnumerableFactory>().ToArray();

        var listStrings = new [] { "abc", "cc", "ABC", "34", "d", "", "g", "tt", "sdfsdf", "9089" };
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
    public void EnumerableFactoryTest() => _enumerableFactoryTester.Test();

    [Test]
    public void DictionaryFactoryTest() => _dictionaryFactoryTester.Test();

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
        var hssFactory = _registry.GetFactory<HashSetStringFactory>();
        Assert.That(hssFactory.Kind.IsProxy(), Is.False);

        var ignoreCase = hssFactory.Comparer == StringComparer.OrdinalIgnoreCase;
        SetStringTest(hssFactory.New(10), ignoreCase);

        var hhsFactory2 = _registry.GetFactory<HashSet<string>, string>();
        Assert.That(ReferenceEquals(hssFactory, hhsFactory2), Is.True);

        var hhsFactory3 = _registry.GetFactory<ISet<string>, string>();
        Assert.That(ReferenceEquals(hssFactory, hhsFactory3), Is.True);
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