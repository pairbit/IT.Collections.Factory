using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace IT.Collections.Factory;

using Factories;
using Generic;

public class EnumerableFactoryRegistry
{
    static readonly ConcurrentDictionary<Type, IEnumerableFactory> _enumerableFactories = new();
    static readonly ConcurrentDictionary<Type, IDictionaryFactory> _dictionaryFactories = new();

    static readonly ConcurrentDictionary<Type, object> _genericEnumerableFactories = new();

    static readonly ReadOnlyDictionary<Type, IEnumerableFactory> _enumerableFactoriesReadOnly = new(_enumerableFactories);
    static readonly ReadOnlyDictionary<Type, IDictionaryFactory> _dictionaryFactoriesReadOnly = new(_dictionaryFactories);

    static EnumerableFactoryRegistry()
    {
        //RegisterEnumerableFactory(ArrayFactory.Default, typeof(IEnumerable<>));
        RegisterEnumerableFactory(ListFactory.Default, typeof(IList<>));
        RegisterEnumerableFactory(LinkedListFactory.Default, typeof(ICollection<>), typeof(IEnumerable<>));
        RegisterEnumerableFactory(HashSetFactory.Default, typeof(ISet<>));
        RegisterEnumerableFactory(SortedSetFactory.Default);
        RegisterEnumerableFactory(StackFactory.Default);
        RegisterEnumerableFactory(QueueFactory.Default);
        RegisterEnumerableFactory(CollectionFactory.Default);
        RegisterEnumerableFactory(ObservableCollectionFactory.Default);
        RegisterEnumerableFactory(ReadOnlyObservableCollectionFactory.Default);

        RegisterEnumerableFactory(ReadOnlyListFactory.Default, typeof(IReadOnlyList<>));
        RegisterEnumerableFactory(ReadOnlyLinkedListFactory.Default, typeof(IReadOnlyCollection<>));
#if NET6_0_OR_GREATER
        RegisterEnumerableFactory(ReadOnlyHashSetFactory.Default, typeof(IReadOnlySet<>));
#endif
#if NETCOREAPP3_1_OR_GREATER
        RegisterEnumerableFactory(ImmutableArrayFactory.Default);
        RegisterEnumerableFactory(ImmutableListFactory.Default, typeof(System.Collections.Immutable.IImmutableList<>));
        RegisterEnumerableFactory(ImmutableHashSetFactory.Default, typeof(System.Collections.Immutable.IImmutableSet<>));
        RegisterEnumerableFactory(ImmutableSortedSetFactory.Default);
        RegisterEnumerableFactory(ImmutableStackFactory.Default, typeof(System.Collections.Immutable.IImmutableStack<>));
        RegisterEnumerableFactory(ImmutableQueueFactory.Default, typeof(System.Collections.Immutable.IImmutableQueue<>));
        RegisterDictionaryFactory(ImmutableDictionaryFactory.Default, typeof(System.Collections.Immutable.IImmutableDictionary<,>));
        RegisterDictionaryFactory(ImmutableSortedDictionaryFactory.Default);
#endif
        RegisterDictionaryFactory(DictionaryFactory.Default, typeof(IDictionary<,>));
        RegisterDictionaryFactory(ReadOnlyDictionaryFactory.Default, typeof(IReadOnlyDictionary<,>));
        RegisterDictionaryFactory(SortedDictionaryFactory.Default);
        RegisterDictionaryFactory(SortedListFactory.Default);

        RegisterDictionaryFactory(ConcurrentDictionaryFactory.Default);
        RegisterEnumerableFactory(ConcurrentBagFactory.Default, typeof(IProducerConsumerCollection<>));
        RegisterEnumerableFactory(ConcurrentQueueFactory.Default);
        RegisterEnumerableFactory(ConcurrentStackFactory.Default);
        RegisterEnumerableFactory(BlockingCollectionFactory.Default);
    }

    public static IReadOnlyDictionary<Type, IEnumerableFactory> EnumerableFactories => _enumerableFactoriesReadOnly;

    public static IReadOnlyDictionary<Type, IDictionaryFactory> DictionaryFactories => _dictionaryFactoriesReadOnly;

    public static void RegisterEnumerableFactory(IEnumerableFactory factory, params Type[] genericTypes)
    {
        if (factory == null) throw new ArgumentNullException(nameof(factory));

        var enumerable = factory.Empty<int>();

        if (enumerable == null) throw new ArgumentException("Factory Error", nameof(factory));

        var enumerableType = enumerable.GetType();

        Type baseType;

        if (enumerableType.IsArray)
        {
            baseType = typeof(Array);
        }
        else
        {
            if (!enumerableType.IsGenericType) throw new ArgumentException($"Registered type '{enumerableType.FullName}' is not generic type", nameof(factory));

            baseType = enumerableType.GetGenericTypeDefinition();

            _enumerableFactories[baseType] = factory;
        }

        if (genericTypes != null && genericTypes.Length > 0)
        {
            for (int i = 0; i < genericTypes.Length; i++)
            {
                var genericType = genericTypes[i];

                if (!genericType.IsGenericType)
                    throw new ArgumentException($"Registered type '{genericType.FullName}' is not generic type", nameof(genericTypes));

                var type = genericType.MakeGenericType(typeof(int));

                if (!typeof(IEnumerable<int>).IsAssignableFrom(type))
                    throw new ArgumentException($"Registered type '{genericType.FullName}' does not inherit type '{typeof(IEnumerable<>).FullName}'", nameof(genericTypes));

                if (!type.IsAssignableFrom(enumerableType))
                    throw new ArgumentException($"Registered type '{genericType.FullName}' is not assignable from type '{baseType.FullName}'", nameof(genericTypes));

                _enumerableFactories[genericType] = factory;
            }
        }
    }

    public static void RegisterEnumerableFactory<TEnumerable, T>(IEnumerableFactory<TEnumerable, T> factory) where TEnumerable : IEnumerable<T>
    {
        if (factory == null) throw new ArgumentNullException(nameof(factory));

        _genericEnumerableFactories[typeof(TEnumerable)] = factory;
    }

    public static void RegisterEnumerableFactory<TEnumerable, T>(EnumerableFactory<TEnumerable, T> factory,
        Func<TEnumerable, T, bool> tryAdd, EnumerableType type = EnumerableType.None) where TEnumerable : IEnumerable<T>
    {
        _genericEnumerableFactories[typeof(TEnumerable)] = new EnumerableFactoryDelegate<TEnumerable, T>(factory, tryAdd, type);
    }

    public static void RegisterEnumerableFactory<TEnumerable, T>(EnumerableFactory<TEnumerable, T> factory,
        Action<TEnumerable, T> add, EnumerableType type = EnumerableType.None) where TEnumerable : IEnumerable<T>
    {
        if (add == null) throw new ArgumentNullException(nameof(add));

        _genericEnumerableFactories[typeof(TEnumerable)] = 
            new EnumerableFactoryDelegate<TEnumerable, T>(factory, (enumerable, item) => { add(enumerable, item); return true; }, type);
    }

    public static void RegisterDictionaryFactory(IDictionaryFactory factory, params Type[] genericTypes)
    {
        if (factory == null) throw new ArgumentNullException(nameof(factory));

        var enumerable = factory.Empty<int, int>();

        if (enumerable == null) throw new ArgumentException("Factory Error", nameof(factory));

        var enumerableType = enumerable.GetType();

        if (!enumerableType.IsGenericType) throw new ArgumentException($"Registered type '{enumerableType.FullName}' is not generic type", nameof(factory));

        var baseType = enumerableType.GetGenericTypeDefinition();

        _dictionaryFactories[baseType] = factory;

        if (genericTypes != null && genericTypes.Length > 0)
        {
            for (int i = 0; i < genericTypes.Length; i++)
            {
                var genericType = genericTypes[i];

                if (!genericType.IsGenericType)
                    throw new ArgumentException($"Registered type '{genericType.FullName}' is not generic type", nameof(genericTypes));

                var type = genericType.MakeGenericType(typeof(int), typeof(int));

                if (!typeof(IEnumerable<KeyValuePair<int, int>>).IsAssignableFrom(type))
                    throw new ArgumentException($"Registered type '{genericType.FullName}' does not inherit type '{typeof(IEnumerable<>).FullName}'", nameof(genericTypes));

                if (!type.IsAssignableFrom(enumerableType))
                    throw new ArgumentException($"Registered type '{genericType.FullName}' is not assignable from type '{baseType.FullName}'", nameof(genericTypes));

                _dictionaryFactories[genericType] = factory;
            }
        }
    }

    public static IEnumerableFactory? TryGetEnumerableFactory(Type genericType)
        => _enumerableFactories.TryGetValue(genericType, out var factory) ? factory : null;

    public static IEnumerableFactory<TEnumerable, T>? TryGetEnumerableFactory<TEnumerable, T>() where TEnumerable : IEnumerable<T>
    {
        var type = typeof(TEnumerable);

        if (_genericEnumerableFactories.TryGetValue(type, out var genericFactory)) return (IEnumerableFactory<TEnumerable, T>)genericFactory;

        if (_enumerableFactories.TryGetValue(type.GetGenericTypeDefinition(), out var factory)) return new EnumerableFactoryProxy<TEnumerable, T>(factory);

        return null;
    }

    public static IEnumerableFactory GetEnumerableFactory(Type genericType)
        => _enumerableFactories.TryGetValue(genericType, out var factory) ? factory : throw new ArgumentException($"EnumerableFactory '{genericType.FullName}' not registered", nameof(genericType));

    public static IEnumerableFactory<TEnumerable, T> GetEnumerableFactory<TEnumerable, T>() where TEnumerable : IEnumerable<T>
        => TryGetEnumerableFactory<TEnumerable, T>() ?? throw new ArgumentException($"EnumerableFactory '{typeof(TEnumerable).FullName}' not registered");

    public static IDictionaryFactory? TryGetDictionaryFactory(Type genericType)
        => _dictionaryFactories.TryGetValue(genericType, out var factory) ? factory : null;

    public static IDictionaryFactory<TDictionary, TKey, TValue>? TryGetDictionaryFactory<TDictionary, TKey, TValue>()
        where TKey : notnull
        where TDictionary : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        var type = typeof(TDictionary);

        if (_genericEnumerableFactories.TryGetValue(type, out var genericFactory)) return (IDictionaryFactory<TDictionary, TKey, TValue>)genericFactory;

        if (_dictionaryFactories.TryGetValue(type.GetGenericTypeDefinition(), out var factory)) return new DictionaryFactoryProxy<TDictionary, TKey, TValue>(factory);

        return null;
    }

    public static IDictionaryFactory GetDictionaryFactory(Type genericType)
        => _dictionaryFactories.TryGetValue(genericType, out var factory) ? factory : throw new ArgumentException($"DictionaryFactory '{genericType.FullName}' not registered", nameof(genericType));

    public static IDictionaryFactory<TDictionary, TKey, TValue> GetDictionaryFactory<TDictionary, TKey, TValue>()
        where TKey : notnull
        where TDictionary : IEnumerable<KeyValuePair<TKey, TValue>>
        => TryGetDictionaryFactory<TDictionary, TKey, TValue>() ?? throw new ArgumentException($"EnumerableFactory '{typeof(TDictionary).FullName}' not registered");
}