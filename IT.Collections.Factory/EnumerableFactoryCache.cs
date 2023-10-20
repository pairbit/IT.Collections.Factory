namespace IT.Collections.Factory;

public static class EnumerableFactoryCache<TFactory> 
    where TFactory : IEnumerableFactoryRegistrable, new()
{
    public static readonly TFactory Default = new();
}