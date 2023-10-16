namespace IT.Collections.Factory.Internal;

internal static class CacheFactory<TFactory> where TFactory : IEnumerableFactoryRegistrable
{
    public static readonly bool IsValid;
    public static readonly string? Error;
    public static readonly Type? EnumerableTypeDefinition;

    static CacheFactory()
    {
        var factoryType = typeof(TFactory);

        var isValid = factoryType.IsAssignableFromEnumerableFactory() ||
                      factoryType.IsAssignableFromDictionaryFactory();

        if (isValid)
        {
            var enumerableTypeDefinition = factoryType.GetEnumerableTypeDefinition();

            if (enumerableTypeDefinition == null)
            {
                Error = $"Type '{typeof(TFactory).FullName}' not contains same return type";
                isValid = false;
            }

            EnumerableTypeDefinition = enumerableTypeDefinition;
        }
        else
        {
            Error = $"Type '{typeof(TFactory).FullName}' not valid";
        }

        IsValid = isValid;
    }
}