namespace IT.Collections.Factory;

using Factories;
using Generic;

public static class xIEnumerableFactoryRegistry
{
    public static bool TryRegisterFactory<TFactory>(this IEnumerableFactoryRegistry registry, RegistrationBehavior behavior) where TFactory : IEnumerableFactoryRegistrable, new()
        => registry.TryRegisterFactory(EnumerableFactoryCache<TFactory>.Default, behavior);

    public static bool TryRegisterFactory<TRegistrable, TFactory>(this IEnumerableFactoryRegistry registry, RegistrationBehavior behavior)
        where TRegistrable : IEnumerableFactoryRegistrable
        where TFactory : TRegistrable, new()
        => registry.TryRegisterFactory<TRegistrable>(EnumerableFactoryCache<TFactory>.Default, behavior);

    public static bool TryRegisterFactory<TEnumerable, T>(this IEnumerableFactoryRegistry registry, IEnumerableFactory<TEnumerable, T> factory, RegistrationBehavior behavior)
        where TEnumerable : IEnumerable<T>
        => registry.TryRegisterFactory(factory, behavior);

    public static bool TryRegisterFactory<TDictionary, TKey, TValue>(
        this IEnumerableFactoryRegistry registry, IDictionaryFactory<TDictionary, TKey, TValue> factory, RegistrationBehavior behavior)
        where TDictionary : IEnumerable<KeyValuePair<TKey, TValue>>
        => registry.TryRegisterFactory(factory, behavior);

    public static bool TryRegisterFactoriesDefaultOnlyClasses(this IEnumerableFactoryRegistry registry, RegistrationBehavior behavior)
        => registry.TryRegisterFactory<ArrayFactory>(behavior) &
           registry.TryRegisterFactory<ListFactory>(behavior) &
           registry.TryRegisterFactory<LinkedListFactory>(behavior) &
           registry.TryRegisterFactory<HashSetFactory>(behavior) &
           registry.TryRegisterFactory<SortedSetFactory>(behavior) &
           registry.TryRegisterFactory<StackFactory>(behavior) &
           registry.TryRegisterFactory<QueueFactory>(behavior) &
           registry.TryRegisterFactory<CollectionFactory>(behavior) &
           registry.TryRegisterFactory<ObservableCollectionFactory>(behavior) &
           registry.TryRegisterFactory<ReadOnlyObservableCollectionFactory>(behavior) &
           registry.TryRegisterFactory<ReadOnlyCollectionFactory>(behavior) &
#if NETCOREAPP3_1_OR_GREATER
           registry.TryRegisterFactory<ImmutableArrayFactory>(behavior) &
           registry.TryRegisterFactory<ImmutableListFactory>(behavior) &
           registry.TryRegisterFactory<ImmutableHashSetFactory>(behavior) &
           registry.TryRegisterFactory<ImmutableSortedSetFactory>(behavior) &
           registry.TryRegisterFactory<ImmutableStackFactory>(behavior) &
           registry.TryRegisterFactory<ImmutableQueueFactory>(behavior) &
           registry.TryRegisterFactory<ImmutableDictionaryFactory>(behavior) &
           registry.TryRegisterFactory<ImmutableSortedDictionaryFactory>(behavior) &
#endif
           registry.TryRegisterFactory<DictionaryFactory>(behavior) &
           registry.TryRegisterFactory<ReadOnlyDictionaryFactory>(behavior) &
           registry.TryRegisterFactory<SortedDictionaryFactory>(behavior) &
           registry.TryRegisterFactory<SortedListFactory>(behavior) &
           registry.TryRegisterFactory<ConcurrentDictionaryFactory>(behavior) &
           registry.TryRegisterFactory<ConcurrentBagFactory>(behavior) &
           registry.TryRegisterFactory<ConcurrentQueueFactory>(behavior) &
           registry.TryRegisterFactory<ConcurrentStackFactory>(behavior) &
           registry.TryRegisterFactory<BlockingCollectionFactory>(behavior);

    public static bool TryRegisterFactoriesDefaultOnlyInterfaces(this IEnumerableFactoryRegistry registry, RegistrationBehavior behavior)
        => registry.TryRegisterFactory<IEnumerableFactory, LinkedListFactory>(behavior) &
           registry.TryRegisterFactory<ICollectionFactory, LinkedListFactory>(behavior) &
           registry.TryRegisterFactory<IListFactory, ListFactory>(behavior) &
           registry.TryRegisterFactory<ISetFactory, HashSetFactory>(behavior) &
           registry.TryRegisterFactory<IReadOnlyCollectionFactory, ReadOnlyCollectionFactoryProxy>(behavior) &
           registry.TryRegisterFactory<IReadOnlyListFactory, ReadOnlyCollectionFactory>(behavior) &
#if NET6_0_OR_GREATER
           registry.TryRegisterFactory<IReadOnlySetFactory, ReadOnlySetFactoryProxy>(behavior) &
#endif
#if NETCOREAPP3_1_OR_GREATER
           registry.TryRegisterFactory<IImmutableListFactory, ImmutableListFactory>(behavior) &
           registry.TryRegisterFactory<IImmutableSetFactory, ImmutableHashSetFactory>(behavior) &
           registry.TryRegisterFactory<IImmutableStackFactory, ImmutableStackFactory>(behavior) &
           registry.TryRegisterFactory<IImmutableQueueFactory, ImmutableQueueFactory>(behavior) &
           registry.TryRegisterFactory<IImmutableDictionaryFactory, ImmutableDictionaryFactory>(behavior) &
#endif
           registry.TryRegisterFactory<IDictionaryFactory, DictionaryFactory>(behavior) &
           registry.TryRegisterFactory<IReadOnlyDictionaryFactory, ReadOnlyDictionaryFactory>(behavior) &
           registry.TryRegisterFactory<IProducerConsumerCollectionFactory, ConcurrentBagFactory>(behavior);

    public static bool TryRegisterFactoriesDefault(this IEnumerableFactoryRegistry registry, RegistrationBehavior behavior)
        => registry.TryRegisterFactoriesDefaultOnlyClasses(behavior) &
           registry.TryRegisterFactoriesDefaultOnlyInterfaces(behavior);

    public static IEnumerableFactoryRegistry RegisterFactory<TFactory>(this IEnumerableFactoryRegistry registry, TFactory factory,
        RegistrationBehavior behavior = RegistrationBehavior.ThrowOnExisting) where TFactory : IEnumerableFactoryRegistrable
    {
        registry.TryRegisterFactory(factory, behavior);
        return registry;
    }

    public static IEnumerableFactoryRegistry RegisterFactory<TFactory>(this IEnumerableFactoryRegistry registry,
        RegistrationBehavior behavior = RegistrationBehavior.ThrowOnExisting) where TFactory : IEnumerableFactoryRegistrable, new()
    {
        registry.TryRegisterFactory<TFactory>(behavior);
        return registry;
    }

    public static IEnumerableFactoryRegistry RegisterFactory<TRegistrable, TFactory>(this IEnumerableFactoryRegistry registry,
        RegistrationBehavior behavior = RegistrationBehavior.ThrowOnExisting)
        where TRegistrable : IEnumerableFactoryRegistrable
        where TFactory : TRegistrable, new()
    {
        registry.TryRegisterFactory<TRegistrable, TFactory>(behavior);
        return registry;
    }

    public static IEnumerableFactoryRegistry RegisterFactoriesDefaultOnlyClasses(this IEnumerableFactoryRegistry registry,
        RegistrationBehavior behavior = RegistrationBehavior.ThrowOnExisting)
    {
        registry.TryRegisterFactoriesDefaultOnlyClasses(behavior);
        return registry;
    }

    public static IEnumerableFactoryRegistry RegisterFactoriesDefaultOnlyInterfaces(this IEnumerableFactoryRegistry registry,
        RegistrationBehavior behavior = RegistrationBehavior.ThrowOnExisting)
    {
        registry.TryRegisterFactoriesDefaultOnlyInterfaces(behavior);
        return registry;
    }

    public static IEnumerableFactoryRegistry RegisterFactoriesDefault(this IEnumerableFactoryRegistry registry,
        RegistrationBehavior behavior = RegistrationBehavior.ThrowOnExisting)
    {
        registry.TryRegisterFactoriesDefault(behavior);
        return registry;
    }
}