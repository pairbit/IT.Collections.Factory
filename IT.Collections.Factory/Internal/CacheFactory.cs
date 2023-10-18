namespace IT.Collections.Factory.Internal;

internal static class CacheFactory<TFactory> where TFactory : IEnumerableFactoryRegistrable
{
    public static readonly bool IsValid;
    public static readonly string? Error;
    public static readonly Type? ReturnType;

    static CacheFactory()
    {
        ReturnType = typeof(TFactory).TryGetReturnType(out Error);
        IsValid = ReturnType != null;
    }
}