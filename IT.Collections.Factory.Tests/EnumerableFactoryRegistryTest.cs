namespace IT.Collections.Factory.Tests;

using Internal;
using Factories;

public class EnumerableFactoryRegistryTest
{
    private readonly static IEnumerableFactoryRegistry Registry = new EnumerableFactoryRegistry(50).RegisterAllDefaultFactories();

    private readonly IEnumerableFactoryRegistry _registry;
    private readonly EnumerableFactoryTester _enumerableFactoryTester;
    private readonly DictionaryFactoryTester _dictionaryFactoryTester;

    public EnumerableFactoryRegistryTest() : this(Registry) { }

    public EnumerableFactoryRegistryTest(IEnumerableFactoryRegistry registry)
    {
        _registry = registry;
        var factories = registry.Values;
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

        var listInt = listFactory.New<int>(10);

        Assert.IsTrue(listInt.Capacity == 10);
    }
}