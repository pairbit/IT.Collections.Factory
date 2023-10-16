namespace IT.Collections.Factory.Tests;

using Internal;
using Factories;

public class EnumerableFactoryRegistryTest
{
    private readonly static IEnumerableFactoryRegistry Registry = new EnumerableFactoryRegistry(50).RegisterFactoriesDefault();

    private readonly IEnumerableFactoryRegistry _registry;
    private readonly EnumerableFactoryTester _enumerableFactoryTester;
    private readonly DictionaryFactoryTester _dictionaryFactoryTester;

    public EnumerableFactoryRegistryTest() : this(Registry) { }

    public EnumerableFactoryRegistryTest(IEnumerableFactoryRegistry registry)
    {
        _registry = registry;
        var factories = registry.Values.Distinct().ToArray();
        _enumerableFactoryTester = new(factories.OfType<IEnumerableFactory>().ToArray());
        _dictionaryFactoryTester = new(factories.OfType<IDictionaryFactory>().ToArray());
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

        GenericEnumerableFactoryTest<int>();
        GenericEnumerableFactoryTest<int?>();
        GenericEnumerableFactoryTest<string>();

        GenericDictionaryFactoryTest<int, int>();
        GenericDictionaryFactoryTest<int, int?>();
        GenericDictionaryFactoryTest<string, string>();
    }

    private void GenericEnumerableFactoryTest<T>()
    {
        var listFactory = _registry.GetFactory<List<T>, T>();
        ListTest(listFactory.New(20), 20);
    }

    private void GenericDictionaryFactoryTest<TKey, TValue>() where TKey : notnull
    {
        var dictionaryFactory = _registry.GetFactory<Dictionary<TKey, TValue>, TKey, TValue>();
        DictionaryTest(dictionaryFactory.New(20));
    }

    private void ListTest<T>(List<T> list, int capacity)
    {
        Assert.That(list.Capacity, Is.EqualTo(capacity));
    }

    private void DictionaryTest<TKey, TValue>(Dictionary<TKey, TValue> dictionary) where TKey : notnull
    {
        Assert.That(dictionary.Count, Is.EqualTo(0));
    }
}