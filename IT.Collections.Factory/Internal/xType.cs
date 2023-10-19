using System.Reflection;

namespace IT.Collections.Factory.Internal;

internal static class xType
{
    private static readonly BindingFlags Bindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
    internal static readonly Type IEnumerableGenericType = typeof(IEnumerable<>);
    private static readonly Type IEnumerableFactoryGenericType = typeof(Generic.IEnumerableFactory<,>);
    private static readonly Type IDictionaryFactoryGenericType = typeof(Generic.IDictionaryFactory<,,>);
    private static readonly Type IEnumerableFactoryType = typeof(IEnumerableFactory);
    private static readonly Type IDictionaryFactoryType = typeof(IEnumerableKeyValueFactory);
    private static readonly Type ArrayType = typeof(Array);

    public static bool IsAssignableToEnumerable(this Type type) => type == ArrayType || type.IsAssignableToDefinition(IEnumerableGenericType);

    public static bool IsAssignableToDefinition(this Type type, Type baseType)
    {
        if (baseType.IsGenericTypeDefinition)
        {
            if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                type = type.GetGenericTypeDefinition();
                if (type == baseType) return true;
            }
            if (baseType.IsInterface)
            {
                var itypes = type.GetInterfaces();
                for (int i = 0; i < itypes.Length; i++)
                {
                    var itype = itypes[i];
                    if (itype.IsGenericType)
                    {
                        if (!itype.IsGenericTypeDefinition)
                            itype = itype.GetGenericTypeDefinition();

                        if (itype == baseType) return true;
                    }
                }

                return false;
            }

            Type? typeBaseType = type.BaseType;
            while (typeBaseType != null)
            {
                if (typeBaseType.IsGenericType)
                {
                    if (!typeBaseType.IsGenericTypeDefinition)
                        typeBaseType = typeBaseType.GetGenericTypeDefinition();

                    if (typeBaseType == baseType) return true;
                }

                typeBaseType = typeBaseType.BaseType;
            }

            return false;
        }

        return baseType.IsAssignableFrom(type);
    }

    public static bool IsGenericMethodParameter(this Type type) => type.IsGenericParameter && type.DeclaringMethod != null;

    public static bool EqualsGenericType(this Type type, Type type2)
    {
        if (type == type2) return true;

        if (type.IsGenericType && type2.IsGenericType)
        {
            var typeDefinition = type.GetGenericTypeDefinition();
            var typeDefinition2 = type2.GetGenericTypeDefinition();
            if (typeDefinition == typeDefinition2)
            {
                var genericArguments = type.GetGenericArguments();
                var genericArguments2 = type2.GetGenericArguments();
                if (genericArguments.Length == genericArguments2.Length)
                {
                    for (int i = 0; i < genericArguments.Length; i++)
                    {
                        var genericArgument = genericArguments[i];
                        var genericArgument2 = genericArguments2[i];

                        if (!genericArgument.IsGenericMethodParameter() ||
                            !genericArgument2.IsGenericMethodParameter())
                            return false;
                    }
                    return true;
                }
            }
        }
        else if (type.IsArray && type2.IsArray)
        {
            var rank = type.GetArrayRank();
            if (rank == type2.GetArrayRank())
            {
                var elementType = type.GetElementType() ?? throw new InvalidOperationException("type.GetElementType() is null");
                var elementType2 = type2.GetElementType() ?? throw new InvalidOperationException("type2.GetElementType() is null");

                return elementType.IsGenericMethodParameter() && elementType2.IsGenericMethodParameter();
            }
        }

        return false;
    }

    public static Type? TryGetReturnType(this Type factoryType, out string? error)
    {
        if (factoryType.IsGenericType)
        {
            var factoryTypeDefinition = factoryType.GetGenericTypeDefinition();
            if (factoryTypeDefinition == IEnumerableFactoryGenericType ||
                factoryTypeDefinition == IDictionaryFactoryGenericType)
            {
                error = null;
                return factoryType.GetGenericArguments()[0];
            }
        }

        if (!IEnumerableFactoryType.IsAssignableFrom(factoryType) &&
            !IDictionaryFactoryType.IsAssignableFrom(factoryType) &&
            !factoryType.IsAssignableToDefinition(IEnumerableFactoryGenericType))
        {
            error = $"Type '{factoryType.FullName}' must implement one of interfaces '{IEnumerableFactoryType.FullName}', '{IDictionaryFactoryType.FullName}', '{IEnumerableFactoryGenericType.FullName}'";
            return null;
        }
        var emptyMethodName = nameof(IEnumerableFactory.Empty);
        var emptyMethod = factoryType.GetMethod(emptyMethodName, Bindings);
        if (emptyMethod == null)
        {
            error = $"Type '{factoryType.FullName}' does not contain method '{emptyMethodName}'";
            return null;
        }

        var returnType = emptyMethod.ReturnType;
        var methods = factoryType.GetMethods(Bindings);
        var newMethodName = nameof(IEnumerableFactory.New);
        var newMethodAny = false;
        var returnTypeSame = true;
        for (int i = 0; i < methods.Length; i++)
        {
            var method = methods[i];
            if (method.Name == newMethodName)
            {
                newMethodAny = true;
                if (!returnType.EqualsGenericType(method.ReturnType))
                {
                    returnTypeSame = false;
                    break;
                }
            }
        }
        if (!newMethodAny)
        {
            error = $"Type '{factoryType.FullName}' does not contain method '{newMethodName}'";
            return null;
        }
        if (!returnTypeSame)
        {
            error = $"Type '{factoryType.FullName}' must contain the same return types of methods '{emptyMethodName}' and '{newMethodName}'";
            return null;
        }

        error = null;
        if (returnType.FullName == null)
        {
            if (returnType.IsArray) return returnType.BaseType;
            if (returnType.IsGenericType) return returnType.GetGenericTypeDefinition();
        }
        return returnType;
    }
}