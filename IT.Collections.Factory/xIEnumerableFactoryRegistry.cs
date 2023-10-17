namespace IT.Collections.Factory;

using Factories;
using Generic;

public static class xIEnumerableFactoryRegistry
{
    public static bool TryRegisterFactory<TEnumerable, T>(this IEnumerableFactoryRegistry registry, IEnumerableFactory<TEnumerable, T> factory, RegistrationBehavior behavior)
        where TEnumerable : IEnumerable<T>
        => registry.TryRegisterFactory(factory, behavior);

    public static bool TryRegisterFactory<TDictionary, TKey, TValue>(
        this IEnumerableFactoryRegistry registry, IDictionaryFactory<TDictionary, TKey, TValue> factory, RegistrationBehavior behavior)
        where TDictionary : IEnumerable<KeyValuePair<TKey, TValue>>
        => registry.TryRegisterFactory(factory, behavior);

    public static bool TryRegisterFactoriesDefault(this IEnumerableFactoryRegistry registry, RegistrationBehavior behavior)
        => registry.TryRegisterFactory(ArrayFactory.Default, behavior) &
           registry.TryRegisterFactory(ListFactory.Default, behavior) &
           registry.TryRegisterFactory(LinkedListFactory.Default, behavior) &
           registry.TryRegisterFactory(HashSetFactory.Default, behavior) &
           registry.TryRegisterFactory(SortedSetFactory.Default, behavior) &
           registry.TryRegisterFactory(StackFactory.Default, behavior) &
           registry.TryRegisterFactory(QueueFactory.Default, behavior) &
           registry.TryRegisterFactory(CollectionFactory.Default, behavior) &
           registry.TryRegisterFactory(ObservableCollectionFactory.Default, behavior) &
           registry.TryRegisterFactory(ReadOnlyObservableCollectionFactory.Default, behavior) &
           registry.TryRegisterFactory(ReadOnlyListFactory.Default, behavior) &
#if NETCOREAPP3_1_OR_GREATER
           registry.TryRegisterFactory(ImmutableArrayFactory.Default, behavior) &
           registry.TryRegisterFactory(ImmutableListFactory.Default, behavior) &
           registry.TryRegisterFactory(ImmutableHashSetFactory.Default, behavior) &
           registry.TryRegisterFactory(ImmutableSortedSetFactory.Default, behavior) &
           registry.TryRegisterFactory(ImmutableStackFactory.Default, behavior) &
           registry.TryRegisterFactory(ImmutableQueueFactory.Default, behavior) &
           registry.TryRegisterFactory(ImmutableDictionaryFactory.Default, behavior) &
           registry.TryRegisterFactory(ImmutableSortedDictionaryFactory.Default, behavior) &
#endif
           registry.TryRegisterFactory(DictionaryFactory.Default, behavior) &
           registry.TryRegisterFactory(ReadOnlyDictionaryFactory.Default, behavior) &
           registry.TryRegisterFactory(SortedDictionaryFactory.Default, behavior) &
           registry.TryRegisterFactory(SortedListFactory.Default, behavior) &
           registry.TryRegisterFactory(ConcurrentDictionaryFactory.Default, behavior) &
           registry.TryRegisterFactory(ConcurrentBagFactory.Default, behavior) &
           registry.TryRegisterFactory(ConcurrentQueueFactory.Default, behavior) &
           registry.TryRegisterFactory(ConcurrentStackFactory.Default, behavior) &
           registry.TryRegisterFactory(BlockingCollectionFactory.Default, behavior);

    public static bool TryRegisterFactoriesDefaultInterfaces(this IEnumerableFactoryRegistry registry, RegistrationBehavior behavior)
        => registry.TryRegisterFactory<IEnumerableFactory>(LinkedListFactory.Default, behavior) &
           registry.TryRegisterFactory<ICollectionFactory>(LinkedListFactory.Default, behavior) &
           registry.TryRegisterFactory<IListFactory>(ListFactory.Default, behavior) &
           registry.TryRegisterFactory<ISetFactory>(HashSetFactory.Default, behavior) &
           registry.TryRegisterFactory<IReadOnlyCollectionFactory>(ReadOnlyLinkedListFactory.Default, behavior) &
           registry.TryRegisterFactory<IReadOnlyListFactory>(ReadOnlyListFactory.Default, behavior) &
#if NET6_0_OR_GREATER
           registry.TryRegisterFactory<IReadOnlySetFactory>(ReadOnlyHashSetFactory.Default, behavior) &
#endif
#if NETCOREAPP3_1_OR_GREATER
           registry.TryRegisterFactory<IImmutableListFactory>(ImmutableListFactory.Default, behavior) &
           registry.TryRegisterFactory<IImmutableSetFactory>(ImmutableHashSetFactory.Default, behavior) &
           registry.TryRegisterFactory<IImmutableStackFactory>(ImmutableStackFactory.Default, behavior) &
           registry.TryRegisterFactory<IImmutableQueueFactory>(ImmutableQueueFactory.Default, behavior) &
           //registry.TryRegisterFactory<IImmutableDictionaryFactory>(ImmutableDictionaryFactory.Default, behavior) &
#endif
           registry.TryRegisterFactory<IProducerConsumerCollectionFactory>(ConcurrentBagFactory.Default, behavior);

    public static bool TryRegisterFactoriesDefaultAndInterfaces(this IEnumerableFactoryRegistry registry, RegistrationBehavior behavior)
        => registry.TryRegisterFactoriesDefault(behavior) &
           registry.TryRegisterFactoriesDefaultInterfaces(behavior);

    public static IEnumerableFactoryRegistry RegisterFactoriesDefault(this IEnumerableFactoryRegistry registry,
        RegistrationBehavior behavior = RegistrationBehavior.ThrowOnExisting)
    {
        registry.TryRegisterFactoriesDefault(behavior);
        return registry;
    }

    public static IEnumerableFactoryRegistry RegisterFactoriesDefaultInterfaces(this IEnumerableFactoryRegistry registry,
        RegistrationBehavior behavior = RegistrationBehavior.ThrowOnExisting)
    {
        registry.TryRegisterFactoriesDefaultInterfaces(behavior);
        return registry;
    }

    public static IEnumerableFactoryRegistry RegisterFactoriesDefaultAndInterfaces(this IEnumerableFactoryRegistry registry,
        RegistrationBehavior behavior = RegistrationBehavior.ThrowOnExisting)
    {
        registry.TryRegisterFactoriesDefaultAndInterfaces(behavior);
        return registry;
    }
}

/*
 
RegisterEnumerableFactory(ListFactory.Default, typeof(IList<>));
RegisterEnumerableFactory(LinkedListFactory.Default, typeof(ICollection<>), typeof(IEnumerable<>));
RegisterEnumerableFactory(HashSetFactory.Default, typeof(ISet<>));

RegisterEnumerableFactory(ReadOnlyListFactory.Default, typeof(IReadOnlyList<>));
RegisterEnumerableFactory(ReadOnlyLinkedListFactory.Default, typeof(IReadOnlyCollection<>));
#if NET6_0_OR_GREATER
RegisterEnumerableFactory(ReadOnlyHashSetFactory.Default, typeof(IReadOnlySet<>));
#endif

RegisterEnumerableFactory(ImmutableListFactory.Default, typeof(System.Collections.Immutable.IImmutableList<>));
RegisterEnumerableFactory(ImmutableHashSetFactory.Default, typeof(System.Collections.Immutable.IImmutableSet<>));
RegisterEnumerableFactory(ImmutableSortedSetFactory.Default);
RegisterEnumerableFactory(ImmutableStackFactory.Default, typeof(System.Collections.Immutable.IImmutableStack<>));
RegisterEnumerableFactory(ImmutableQueueFactory.Default, typeof(System.Collections.Immutable.IImmutableQueue<>));
RegisterDictionaryFactory(ImmutableDictionaryFactory.Default, typeof(System.Collections.Immutable.IImmutableDictionary<,>));

RegisterDictionaryFactory(DictionaryFactory.Default, typeof(IDictionary<,>));
RegisterDictionaryFactory(ReadOnlyDictionaryFactory.Default, typeof(IReadOnlyDictionary<,>));
RegisterDictionaryFactory(SortedDictionaryFactory.Default);
RegisterDictionaryFactory(SortedListFactory.Default);

RegisterDictionaryFactory(ConcurrentDictionaryFactory.Default);
RegisterEnumerableFactory(ConcurrentBagFactory.Default, typeof(IProducerConsumerCollection<>));
 */