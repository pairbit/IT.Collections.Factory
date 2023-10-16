﻿using IT.Collections.Factory;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System;

internal static class xType
{
    public static readonly Type IEnumerableFactoryType = typeof(IEnumerableFactory);
    public static readonly Type IDictionaryFactoryType = typeof(IDictionaryFactory);

#if NET5_0_OR_GREATER
    public static readonly Type EnumerableFactoryType = typeof(EnumerableFactory);
    public static readonly Type BaseDictionaryFactoryType = typeof(BaseDictionaryFactory);
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAssignableFromEnumerableFactory(this Type type) => IEnumerableFactoryType.IsAssignableFrom(type);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAssignableFromDictionaryFactory(this Type type) => IDictionaryFactoryType.IsAssignableFrom(type);

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
                var elementType = type.GetElementType();
                var elementType2 = type2.GetElementType();

                return elementType.IsGenericMethodParameter() && elementType2.IsGenericMethodParameter();
            }
        }

        return false;
    }

    public static Type? GetEnumerableTypeDefinition(this Type factoryType)
    {
        if (factoryType.IsAssignableFromEnumerableFactory() || factoryType.IsAssignableFromDictionaryFactory())
        {
            var emptyMethod = factoryType.GetMethod(nameof(IEnumerableFactory.Empty)) ?? throw new InvalidOperationException($"Method '{factoryType.FullName}.{nameof(IEnumerableFactory.Empty)}' not found");

            var returnType = emptyMethod.ReturnType;

            var methods = factoryType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            var methodName = nameof(IEnumerableFactory.New);

            for (int i = 0; i < methods.Length; i++)
            {
                var method = methods[i];
                if (method.Name == methodName &&
#if NET5_0_OR_GREATER
                    method.DeclaringType != EnumerableFactoryType && method.DeclaringType != BaseDictionaryFactoryType &&
#endif
                    !returnType.EqualsGenericType(method.ReturnType))
                    return null;
            }

            return returnType.GetGenericTypeDefinitionOrArray();
        }

        return null;
    }
}