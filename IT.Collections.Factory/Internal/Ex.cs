namespace IT.Collections.Factory.Internal;

internal static class Ex
{
    public static ArgumentException NotAssignableFromEnumerableFactory(Type type, string? paramName = null)
        => NotAssignableFrom(type, xType.IEnumerableFactoryType, paramName);

    public static ArgumentException NotAssignableFrom(Type type, Type notAssignableType, string? paramName = null)
        => new ArgumentException($"Type '{type.FullName}' is not assignable from type '{notAssignableType.FullName}'", paramName);
}