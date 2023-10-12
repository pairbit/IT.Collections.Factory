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
        RegisterEnumerable(ListFactory.Default, typeof(IList<>));
        RegisterEnumerable(LinkedListFactory.Default, typeof(ICollection<>), typeof(IEnumerable<>));
        RegisterEnumerable(HashSetFactory.Default, typeof(ISet<>));
        RegisterEnumerable(SortedSetFactory.Default);
        RegisterEnumerable(StackFactory.Default);
        RegisterEnumerable(QueueFactory.Default);
        RegisterEnumerable(CollectionFactory.Default);
        RegisterEnumerable(ObservableCollectionFactory.Default);
        RegisterEnumerable(ReadOnlyObservableCollectionFactory.Default);

        RegisterEnumerable(ReadOnlyListFactory.Default, typeof(IReadOnlyList<>));
        RegisterEnumerable(ReadOnlyLinkedListFactory.Default, typeof(IReadOnlyCollection<>));
#if NET6_0_OR_GREATER
        RegisterEnumerable(ReadOnlyHashSetFactory.Default, typeof(IReadOnlySet<>));
#endif
#if NETCOREAPP3_1_OR_GREATER
        RegisterEnumerable(ImmutableArrayFactory.Default);
        RegisterEnumerable(ImmutableListFactory.Default, typeof(System.Collections.Immutable.IImmutableList<>));
        RegisterEnumerable(ImmutableHashSetFactory.Default, typeof(System.Collections.Immutable.IImmutableSet<>));
        RegisterEnumerable(ImmutableSortedSetFactory.Default);
        RegisterEnumerable(ImmutableStackFactory.Default, typeof(System.Collections.Immutable.IImmutableStack<>));
        RegisterEnumerable(ImmutableQueueFactory.Default, typeof(System.Collections.Immutable.IImmutableQueue<>));
        RegisterDictionary(ImmutableDictionaryFactory.Default, typeof(System.Collections.Immutable.IImmutableDictionary<,>));
        RegisterDictionary(ImmutableSortedDictionaryFactory.Default);
#endif
        RegisterDictionary(DictionaryFactory.Default, typeof(IDictionary<,>));
        RegisterDictionary(ReadOnlyDictionaryFactory.Default, typeof(IReadOnlyDictionary<,>));
        RegisterDictionary(SortedDictionaryFactory.Default);
        RegisterDictionary(SortedListFactory.Default);

        RegisterDictionary(ConcurrentDictionaryFactory.Default);
        RegisterEnumerable(ConcurrentBagFactory.Default, typeof(IProducerConsumerCollection<>));
        RegisterEnumerable(ConcurrentQueueFactory.Default);
        RegisterEnumerable(ConcurrentStackFactory.Default);
        RegisterEnumerable(BlockingCollectionFactory.Default);
    }

    public static void RegisterEnumerable(IEnumerableFactory factory, params Type[] genericTypes)
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

    public static void RegisterDictionary(IDictionaryFactory factory, params Type[] genericTypes)
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
}