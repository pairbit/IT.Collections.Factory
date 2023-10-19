namespace IT.Collections.Factory;

using Generic;
using Internal;

public static class xIReadOnlyEnumerableFactoryRegistry
{
    public static TFactory GetFactory<TFactory>(this IReadOnlyEnumerableFactoryRegistry registry)
        where TFactory : IEnumerableFactoryRegistrable
        => registry.TryGetFactory<TFactory>() ?? throw Ex.FactoryTypeNotRegistered(typeof(TFactory));

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

    internal static IEnumerableFactoryRegistrable? TryGetFactoryProxy(
        this IReadOnlyDictionary<Type, IEnumerableFactoryRegistrable> dictionary, Type factoryType)
    {
        if (factoryType.IsGenericType)
        {
            var factoryTypeDefinition = factoryType.GetGenericTypeDefinition();
            if (factoryTypeDefinition == typeof(IEnumerableFactory<,>))
            {
                var factoryTypeGenericArguments = factoryType.GetGenericArguments();
                var enumerableType = factoryTypeGenericArguments[0];

                if (dictionary.TryGetValue(enumerableType, out var factory)) return factory;
                if (dictionary.TryGetValue(enumerableType.GetGenericTypeDefinition(), out factory) &&
                    factory is IEnumerableFactory enumerableFactory)
                {
                    var proxy = typeof(EnumerableFactoryProxy<,>).MakeGenericType(factoryTypeGenericArguments);
                    return (IEnumerableFactoryRegistrable?)Activator.CreateInstance(proxy, enumerableFactory);
                }
            }
            else if (factoryTypeDefinition == typeof(IDictionaryFactory<,,>))
            {
                var factoryTypeGenericArguments = factoryType.GetGenericArguments();
                var enumerableType = factoryTypeGenericArguments[0];

                if (dictionary.TryGetValue(enumerableType, out var factory)) return factory;
                if (dictionary.TryGetValue(enumerableType.GetGenericTypeDefinition(), out factory) &&
                    factory is IEnumerableKeyValueFactory enumerableKeyValueFactory)
                {
                    var proxy = typeof(DictionaryFactoryProxy<,,>).MakeGenericType(factoryTypeGenericArguments);
                    return (IEnumerableFactoryRegistrable?)Activator.CreateInstance(proxy, enumerableKeyValueFactory);
                }
            }
        }

        return null;
    }
}