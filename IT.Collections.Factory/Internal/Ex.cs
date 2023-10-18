namespace IT.Collections.Factory.Internal;

internal static class Ex
{
    public static ArgumentException EnumerableTypeIsNull(string? paramName = null) => new("EnumerableType is null", paramName);

    public static ArgumentException EnumerableTypeNotAssignableToReturnType(Type enumerableType, Type returnType, string? paramName = null)
        => new($"EnumerableType '{enumerableType.FullName}' does not assignable to return type '{returnType.FullName}'", paramName);

    public static ArgumentException FactoryTypeRegistered(Type factoryType, Type type, string? paramName = null)
        => new($"Factory '{factoryType.FullName}' with type '{type.FullName}' is already registered", paramName);

    //public static ArgumentException NotAssignableFromEnumerableFactory(Type type, string? paramName = null)
    //    => NotAssignableFrom(type, xType.IEnumerableFactoryType, paramName);

    //public static ArgumentException NotAssignableFrom(Type type, Type notAssignableType, string? paramName = null)
    //    => new ArgumentException($"Type '{type.FullName}' is not assignable from type '{notAssignableType.FullName}'", paramName);
}