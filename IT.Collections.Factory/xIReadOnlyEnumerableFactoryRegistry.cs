﻿namespace IT.Collections.Factory;

public static class xIReadOnlyEnumerableFactoryRegistry
{
    

    public static TFactory? TryGetFactory<TFactory>(this IReadOnlyEnumerableFactoryRegistry registry) where TFactory : IEnumerableFactory
    {
        if (registry == null) throw new ArgumentNullException(nameof(registry));

        var type = typeof(TFactory);

        return registry.TryGetValue(type, out var factory) ? (TFactory?)factory : default;
    }

    public static TFactory GetFactory<TFactory>(this IReadOnlyEnumerableFactoryRegistry registry) where TFactory : IEnumerableFactory
        => registry.TryGetFactory<TFactory>() ?? throw new ArgumentException($"Factory type '{typeof(TFactory).FullName}' not registered");

    //private static IEnumerableFactory? TryGetFactoryByType(this IReadOnlyEnumerableFactoryRegistry registry, Type factoryType)
    //{
    //    if (registry == null) throw new ArgumentNullException(nameof(registry));
    //    if (factoryType == null) throw new ArgumentNullException(nameof(factoryType));
    //    if (!factoryType.IsAssignableFrom(IEnumerableFactoryType)) throw new ArgumentException($"Type '{factoryType.FullName}' is not assignable from type '{IEnumerableFactoryType.FullName}'", nameof(factoryType));

    //    return registry.TryGetValue(factoryType, out var factory) ? (IEnumerableFactory)factory : null;
    //}

    //private static IEnumerableFactory? TryGetFactoryByEnumerableTypeDefinition(this IReadOnlyEnumerableFactoryRegistry registry, Type enumerableTypeDefinition)
    //{
    //    if (registry == null) throw new ArgumentNullException(nameof(registry));
    //    if (enumerableTypeDefinition == null) throw new ArgumentNullException(nameof(enumerableTypeDefinition));
    //    //if (!enumerableTypeDefinition.IsAssignableFrom(IEnumerableFactoryType)) throw new ArgumentException($"Type '{factoryType.FullName}' is not assignable from type '{IEnumerableFactoryType.FullName}'", nameof(factoryType));

    //    return registry.TryGetValue(enumerableTypeDefinition, out var factory) ? (IEnumerableFactory)factory : null;
    //}

    //private static object? TryGetFactoryByEnumerableType(this IReadOnlyEnumerableFactoryRegistry registry, Type enumerableType)
    //{
    //    if (registry == null) throw new ArgumentNullException(nameof(registry));
    //    if (enumerableType == null) throw new ArgumentNullException(nameof(enumerableType));

    //    return registry.TryGetValue(enumerableType, out var factory) ? factory : null;
    //}
}