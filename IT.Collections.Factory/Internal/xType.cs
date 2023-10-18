﻿using System.Reflection;

namespace IT.Collections.Factory.Internal;

internal static class xType
{
    private static readonly BindingFlags Bindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
    private static readonly Type IEnumerableFactoryType = typeof(IEnumerableFactory);
    private static readonly Type IDictionaryFactoryType = typeof(IEnumerableKeyValueFactory);
    private static readonly Type ArrayType = typeof(Array);

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
        if (!IEnumerableFactoryType.IsAssignableFrom(factoryType) &&
            !IDictionaryFactoryType.IsAssignableFrom(factoryType))
        {
            error = $"Type '{factoryType.FullName}' must implement one of interfaces '{IEnumerableFactoryType.FullName}', '{IDictionaryFactoryType.FullName}'";
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

        if (returnType.IsArray) return ArrayType;
        if (returnType.IsGenericType) return returnType.GetGenericTypeDefinition();
        return returnType;
    }
}