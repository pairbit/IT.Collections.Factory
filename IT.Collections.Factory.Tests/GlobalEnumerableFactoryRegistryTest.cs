namespace IT.Collections.Factory.Tests;

public class GlobalEnumerableFactoryRegistryTest : EnumerableFactoryRegistryTest
{
    private static readonly IEnumerableFactoryRegistry Registry = 
        EnumerableFactoryRegistry.Global.RegisterFactoriesDefaultAndInterfaces();

    public GlobalEnumerableFactoryRegistryTest() : base(Registry)
    {

    }
}