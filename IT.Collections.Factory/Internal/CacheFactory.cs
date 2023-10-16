namespace IT.Collections.Factory.Internal;

internal static class CacheFactory<TFactory> where TFactory : IEnumerableFactoryRegistrable
{
    public static readonly bool IsValid;
    public static readonly Type? EnumerableTypeDefinition;

    static CacheFactory()
    {
        var factoryType = typeof(TFactory);

        var isValid = factoryType.IsAssignableFromEnumerableFactory() ||
                      factoryType.IsAssignableFromDictionaryFactory();

        if (isValid) EnumerableTypeDefinition = factoryType.GetEnumerableTypeDefinition();

        IsValid = isValid;
    }
}