using IT.Collections.Factory;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System;

internal static class xType
{
    private static readonly BindingFlags Bindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
    public static readonly Type IEnumerableFactoryType = typeof(IEnumerableFactory);
    public static readonly Type IDictionaryFactoryType = typeof(IEnumerableKeyValueFactory);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAssignableToEnumerableFactory(this Type type) => IEnumerableFactoryType.IsAssignableFrom(type);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAssignableToDictionaryFactory(this Type type) => IDictionaryFactoryType.IsAssignableFrom(type);

    public static bool IsAssignableToDefinition(this Type type, Type baseType)
    {
        if (type.IsGenericTypeDefinition && baseType.IsGenericTypeDefinition)
        {
            var typeArgumentsLength = type.GetGenericArguments().Length;
            if (typeArgumentsLength != baseType.GetGenericArguments().Length) return false;

            var typeArguments = new Type[typeArgumentsLength];
            var intType = typeof(int);
            for (int i = 0; i < typeArguments.Length; i++) typeArguments[i] = intType;

            type = type.MakeGenericType(typeArguments);
            baseType = baseType.MakeGenericType(typeArguments);
        }

        return baseType.IsAssignableFrom(type);
    }

    public static Type GetGenericTypeDefinitionOrArray(this Type type)
        => type.IsArray ? typeof(Array) : type.GetGenericTypeDefinition();

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

    public static Type? GetReturnType(this Type factoryType)
    {
        if (factoryType.IsAssignableToEnumerableFactory() || factoryType.IsAssignableToDictionaryFactory())
        {
            var emptyMethod = factoryType.GetMethod(nameof(IEnumerableFactory.Empty), Bindings) ?? throw new InvalidOperationException($"Method '{factoryType.FullName}.{nameof(IEnumerableFactory.Empty)}' not found");

            var returnType = emptyMethod.ReturnType;

            var methods = factoryType.GetMethods(Bindings);
            var methodName = nameof(IEnumerableFactory.New);

            for (int i = 0; i < methods.Length; i++)
            {
                var method = methods[i];
                if (method.Name == methodName && !returnType.EqualsGenericType(method.ReturnType))
                    return null;
            }

            return returnType.GetGenericTypeDefinitionOrArray();
        }

        return null;
    }
}