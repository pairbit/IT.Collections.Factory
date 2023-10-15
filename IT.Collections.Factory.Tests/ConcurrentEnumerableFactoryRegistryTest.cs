namespace IT.Collections.Factory.Tests;

public class ConcurrentEnumerableFactoryRegistryTest : EnumerableFactoryRegistryTest
{
    private static readonly IEnumerableFactoryRegistry _registry =
        new ConcurrentEnumerableFactoryRegistry(-1, 50).RegisterAllDefaultFactories();

    public ConcurrentEnumerableFactoryRegistryTest() : base(_registry)
    {

    }
}