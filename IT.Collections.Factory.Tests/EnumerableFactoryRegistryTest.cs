namespace IT.Collections.Factory.Tests;

using Internal;

public class EnumerableFactoryRegistryTest
{
    private readonly static EnumerableFactoryRegistry _registry;

    private readonly EnumerableFactoryTester _enumerableFactoryTester;
    private readonly DictionaryFactoryTester _dictionaryFactoryTester;

    static EnumerableFactoryRegistryTest()
    {
        var registry = new EnumerableFactoryRegistry(50);

        registry.TryRegisterAllDefaultFactories(RegistrationBehavior.ThrowOnExisting);

        _registry = registry;
    }

    public EnumerableFactoryRegistryTest() : this(_registry) { }

    public EnumerableFactoryRegistryTest(IEnumerableFactoryRegistry registry)
    {
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
}