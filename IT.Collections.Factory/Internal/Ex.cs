namespace IT.Collections.Factory.Internal;

internal static class Ex
{
    public static ArgumentException EnumerableTypeIsNull(Type factoryType, string? paramName = null)
        => new($"Property '{nameof(IEnumerableFactoryRegistrable.EnumerableType)}' of type '{factoryType.FullName}' must not be null", paramName);

    public static ArgumentException EnumerableTypeNotInheritedFromReturnType(Type enumerableType, Type returnType, string? paramName = null)
        => new($"EnumerableType '{enumerableType.FullName}' must inherit return type '{returnType.FullName}'", paramName);
    //TODO: Указать тип фабрики в ошибке
    //is not inherited from

    public static ArgumentException FactoryTypeRegistered(Type factoryType, Type type, string? paramName = null)
        => new($"Factory '{factoryType.FullName}' with type '{type.FullName}' is already registered", paramName);

    public static ArgumentException FactoryTypeNotRegistered(Type type, string? paramName = null)
        => new($"Factory '{type.FullName}' is not registered", paramName);

    public static ArgumentOutOfRangeException BehaviorInvalid(RegistrationBehavior behavior, string? paramName = null)
        => new(paramName, behavior, "RegistrationBehavior invalid");
}