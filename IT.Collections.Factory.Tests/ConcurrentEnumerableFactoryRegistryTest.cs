namespace IT.Collections.Factory.Tests;

public class ConcurrentEnumerableFactoryRegistryTest : EnumerableFactoryRegistryTest
{
    private static readonly IEnumerableFactoryRegistry Registry =
        new ConcurrentEnumerableFactoryRegistry();

    public ConcurrentEnumerableFactoryRegistryTest() : base(Registry) { }
}