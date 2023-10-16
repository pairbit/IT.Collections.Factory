namespace IT.Collections.Factory;

using Factories;
using Internal;
using Generic;

public static class xIEnumerableFactoryRegistry
{
    //public static bool TryRegister(this IEnumerableFactoryRegistry registry, Type factoryType, IEnumerableFactory factory, RegistrationBehavior behavior)
    //{
    //    if (registry == null) throw new ArgumentNullException(nameof(registry));
    //    if (factory == null) throw new ArgumentNullException(nameof(factory));
    //    if (!factoryType.IsAssignableFromEnumerableFactory()) throw Ex.NotAssignableFromEnumerableFactory(factoryType, nameof(factoryType));

    //    return registry.TryRegister(factoryType, factory, behavior);
    //}

    //public static bool TryRegister<TFactory>(this IEnumerableFactoryRegistry registry, TFactory factory, RegistrationBehavior behavior) where TFactory : IEnumerableFactory
    //{
    //    if (registry == null) throw new ArgumentNullException(nameof(registry));
    //    if (factory == null) throw new ArgumentNullException(nameof(factory));

    //    return registry.TryRegister(typeof(TFactory), factory, behavior);
    //}

    //public static bool TryReg<TFactory, TEnumerable, T>(this IEnumerableFactoryRegistry registry, TFactory factory, RegistrationBehavior behavior)
    //    where TEnumerable : IEnumerable<T>
    //{
    //}

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
           registry.TryRegisterFactory(ReadOnlyLinkedListFactory.Default, behavior) &
#if NET6_0_OR_GREATER
           registry.TryRegisterFactory(ReadOnlyHashSetFactory.Default, behavior) &
#endif
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

    public static IEnumerableFactoryRegistry RegisterFactoriesDefault(this IEnumerableFactoryRegistry registry,
        RegistrationBehavior behavior = RegistrationBehavior.ThrowOnExisting)
    {
        registry.TryRegisterFactoriesDefault(behavior);
        return registry;
    }

    //public static bool TryRegisterFactoriesDefault(this IEnumerableFactoryRegistry registry, RegistrationBehavior behavior)
    //    => registry.TryRegister(typeof(List<>), ListFactory.Default, behavior) &
    //       registry.TryRegister(typeof(IList<>), ListFactory.Default, behavior) &
    //       registry.TryRegister(typeof(LinkedList<>), LinkedListFactory.Default, behavior) &
    //       registry.TryRegister(typeof(ICollection<>), LinkedListFactory.Default, behavior) &
    //       registry.TryRegister(typeof(HashSet<>), HashSetFactory.Default, behavior) &
    //       registry.TryRegister(typeof(ISet<>), HashSetFactory.Default, behavior) &
    //       registry.TryRegister(SortedSetFactory.Default, behavior);
}