namespace IT.Collections.Factory.Tests;

public class GlobalEnumerableFactoryRegistryTest : EnumerableFactoryRegistryTest
{
    private static readonly IEnumerableFactoryRegistry Registry = 
        EnumerableFactoryRegistry.Global;

    public GlobalEnumerableFactoryRegistryTest() : base(Registry)
    {

    }
}