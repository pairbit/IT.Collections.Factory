using System.Collections.Concurrent;

namespace IT.Collections.Factory;

using Factories;

public class EnumerableFactoryRegistry
{
    static readonly ConcurrentDictionary<Type, IEnumerableFactory> _enumerableTypes = new();
    static readonly ConcurrentDictionary<Type, IDictionaryFactory> _dictionaryTypes = new();

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

            _enumerableTypes[baseType] = factory;
        }

        if (genericTypes != null && genericTypes.Length > 0)
        {
            for (int i = 0; i < genericTypes.Length; i++)
            {
                var genericTypeBase = genericTypes[i];

                if (!genericTypeBase.IsGenericType)
                    throw new ArgumentException($"Registered type '{genericTypeBase.FullName}' is not generic type", nameof(genericTypes));

                if (!genericTypeBase.MakeGenericType(typeof(int)).IsAssignableFrom(enumerableType))
                    throw new ArgumentException($"Registered type '{genericTypeBase.FullName}' is not assignable from type '{baseType.FullName}'", nameof(genericTypes));

                _enumerableTypes[genericTypeBase] = factory;
            }
        }
    }

    public static void RegisterDictionaryFactory(IDictionaryFactory factory, params Type[] genericTypes)
    {
        if (factory == null) throw new ArgumentNullException(nameof(factory));

        var enumerable = factory.Empty<int, int>();

        if (enumerable == null) throw new ArgumentException("Factory Error", nameof(factory));

        var enumerableType = enumerable.GetType();

        if (!enumerableType.IsGenericType) throw new ArgumentException($"Registered type '{enumerableType.FullName}' is not generic type", nameof(factory));

        var baseType = enumerableType.GetGenericTypeDefinition();

        _dictionaryTypes[baseType] = factory;

        if (genericTypes != null && genericTypes.Length > 0)
        {
            for (int i = 0; i < genericTypes.Length; i++)
            {
                var genericTypeBase = genericTypes[i];

                if (!genericTypeBase.IsGenericType)
                    throw new ArgumentException($"Registered type '{genericTypeBase.FullName}' is not generic type", nameof(genericTypes));

                if (!genericTypeBase.MakeGenericType(typeof(int), typeof(int)).IsAssignableFrom(enumerableType))
                    throw new ArgumentException($"Registered type '{genericTypeBase.FullName}' is not assignable from type '{baseType.FullName}'", nameof(genericTypes));

                _dictionaryTypes[genericTypeBase] = factory;
            }
        }
    }

    public static IEnumerableFactory? TryGetEnumerableFactory(Type genericType)
        => _enumerableTypes.TryGetValue(genericType, out var factory) ? factory : null;

    public static IEnumerableFactory GetEnumerableFactory(Type genericType)
        => _enumerableTypes.TryGetValue(genericType, out var factory) ? factory : throw new ArgumentException("EnumerableFactory not registered", nameof(genericType));

    public static IDictionaryFactory? TryGetDictionaryFactory(Type genericType)
        => _dictionaryTypes.TryGetValue(genericType, out var factory) ? factory : null;

    public static IDictionaryFactory GetDictionaryFactory(Type genericType)
        => _dictionaryTypes.TryGetValue(genericType, out var factory) ? factory : throw new ArgumentException("DictionaryFactory not registered", nameof(genericType));
}