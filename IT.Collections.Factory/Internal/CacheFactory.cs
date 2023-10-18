namespace IT.Collections.Factory.Internal;

internal static class CacheFactory<TFactory> where TFactory : IEnumerableFactoryRegistrable
{
    public static readonly bool IsValid;
    public static readonly string? Error;
    public static readonly Type? ReturnType;

    static CacheFactory()
    {
        var factoryType = typeof(TFactory);

        var isValid = factoryType.IsAssignableToEnumerableFactory() ||
                      factoryType.IsAssignableToDictionaryFactory();

        if (isValid)
        {
            var returnType = factoryType.GetReturnType();

            if (returnType == null)
            {
                Error = $"Type '{typeof(TFactory).FullName}' not contains same return type";
                isValid = false;
            }

            ReturnType = returnType;
        }
        else
        {
            Error = $"Type '{typeof(TFactory).FullName}' not valid";
        }

        IsValid = isValid;
    }
}