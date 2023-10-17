namespace IT.Collections.Factory;

using Generic;

public static class xIReadOnlyEnumerableFactoryRegistry
{
    public static TFactory GetFactory<TFactory>(this IReadOnlyEnumerableFactoryRegistry registry)
        where TFactory : IEnumerableFactoryRegistrable
        => registry.TryGetFactory<TFactory>() ?? throw new ArgumentException($"Factory '{typeof(TFactory).FullName}' not registered");

    public static IEnumerableFactory<TEnumerable, T>? TryGetFactory<TEnumerable, T>(this IReadOnlyEnumerableFactoryRegistry registry)
        where TEnumerable : IEnumerable<T>
        => registry.TryGetFactory<IEnumerableFactory<TEnumerable, T>>();

    public static IEnumerableFactory<TEnumerable, T> GetFactory<TEnumerable, T>(this IReadOnlyEnumerableFactoryRegistry registry)
        where TEnumerable : IEnumerable<T>
        => registry.GetFactory<IEnumerableFactory<TEnumerable, T>>();

    public static IDictionaryFactory<TDictionary, TKey, TValue>? TryGetFactory<TDictionary, TKey, TValue>(this IReadOnlyEnumerableFactoryRegistry registry)
        where TDictionary : IEnumerable<KeyValuePair<TKey, TValue>>
        => registry.TryGetFactory<IDictionaryFactory<TDictionary, TKey, TValue>>();

    public static IDictionaryFactory<TDictionary, TKey, TValue> GetFactory<TDictionary, TKey, TValue>(this IReadOnlyEnumerableFactoryRegistry registry)
        where TDictionary : IEnumerable<KeyValuePair<TKey, TValue>>
        => registry.GetFactory<IDictionaryFactory<TDictionary, TKey, TValue>>();

    internal static IEnumerableFactoryRegistrable? TryGetFactoryProxy(this IReadOnlyEnumerableFactoryRegistry registry, Type factoryType)
    {
        if (factoryType.IsGenericType)
        {
            var factoryTypeDefinition = factoryType.GetGenericTypeDefinition();
            if (factoryTypeDefinition == typeof(IEnumerableFactory<,>))
            {
                var factoryTypeGenericArguments = factoryType.GetGenericArguments();
                var enumerableTypeDefinition = factoryTypeGenericArguments[0].GetGenericTypeDefinition();

                if (registry.TryGetValue(enumerableTypeDefinition, out var factory))
                {
                    var proxy = typeof(EnumerableFactoryProxy<,>).MakeGenericType(factoryTypeGenericArguments);
                    return (IEnumerableFactoryRegistrable?)Activator.CreateInstance(proxy, (IEnumerableFactory)factory);
                }
            }
            else if (factoryTypeDefinition == typeof(IDictionaryFactory<,,>))
            {
                var factoryTypeGenericArguments = factoryType.GetGenericArguments();
                var enumerableTypeDefinition = factoryTypeGenericArguments[0].GetGenericTypeDefinition();

                if (registry.TryGetValue(enumerableTypeDefinition, out var factory))
                {
                    var proxy = typeof(DictionaryFactoryProxy<,,>).MakeGenericType(factoryTypeGenericArguments);
                    return (IEnumerableFactoryRegistrable?)Activator.CreateInstance(proxy, (IEnumerableKeyValueFactory)factory);
                }
            }
        }

        return null;
    }
}