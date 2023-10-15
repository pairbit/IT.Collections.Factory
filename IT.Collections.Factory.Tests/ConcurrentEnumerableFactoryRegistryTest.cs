namespace IT.Collections.Factory.Tests;

public class ConcurrentEnumerableFactoryRegistryTest : EnumerableFactoryRegistryTest
{
    private static readonly ConcurrentEnumerableFactoryRegistry _registry;

    static ConcurrentEnumerableFactoryRegistryTest()
    {
        var registry = new ConcurrentEnumerableFactoryRegistry(-1, 50);

        registry.TryRegisterAllDefaultFactories(RegistrationBehavior.ThrowOnExisting);

        _registry = registry;
    }

    public ConcurrentEnumerableFactoryRegistryTest() : base(_registry)
    {

    }
}