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

    public static bool TryRegister<TEnumerable, T>(this IEnumerableFactoryRegistry registry, IEnumerableFactory<TEnumerable, T> factory, RegistrationBehavior behavior)
        where TEnumerable : IEnumerable<T>
        => registry.TryRegister(factory, behavior);

    public static bool TryRegister<TDictionary, TKey, TValue>(
        this IEnumerableFactoryRegistry registry, IDictionaryFactory<TDictionary, TKey, TValue> factory, RegistrationBehavior behavior)
        where TDictionary : IEnumerable<KeyValuePair<TKey, TValue>>
        => registry.TryRegister(factory, behavior);

    public static bool TryRegisterAllDefaultFactories(this IEnumerableFactoryRegistry registry, RegistrationBehavior behavior)
        => registry.TryRegister(ArrayFactory.Default, behavior) &
           registry.TryRegister(ListFactory.Default, behavior) &
           registry.TryRegister(LinkedListFactory.Default, behavior) &
           registry.TryRegister(HashSetFactory.Default, behavior) &
           registry.TryRegister(SortedSetFactory.Default, behavior) &
           registry.TryRegister(StackFactory.Default, behavior) &
           registry.TryRegister(QueueFactory.Default, behavior) &
           registry.TryRegister(CollectionFactory.Default, behavior) &
           registry.TryRegister(ObservableCollectionFactory.Default, behavior) &
           registry.TryRegister(ReadOnlyObservableCollectionFactory.Default, behavior) &
           registry.TryRegister(ReadOnlyListFactory.Default, behavior) &
           registry.TryRegister(ReadOnlyLinkedListFactory.Default, behavior) &
#if NET6_0_OR_GREATER
           registry.TryRegister(ReadOnlyHashSetFactory.Default, behavior) &
#endif
#if NETCOREAPP3_1_OR_GREATER
           registry.TryRegister(ImmutableArrayFactory.Default, behavior) &
           registry.TryRegister(ImmutableListFactory.Default, behavior) &
           registry.TryRegister(ImmutableHashSetFactory.Default, behavior) &
           registry.TryRegister(ImmutableSortedSetFactory.Default, behavior) &
           registry.TryRegister(ImmutableStackFactory.Default, behavior) &
           registry.TryRegister(ImmutableQueueFactory.Default, behavior) &
           registry.TryRegister(ImmutableDictionaryFactory.Default, behavior) &
           registry.TryRegister(ImmutableSortedDictionaryFactory.Default, behavior) &
#endif
           registry.TryRegister(DictionaryFactory.Default, behavior) &
           registry.TryRegister(ReadOnlyDictionaryFactory.Default, behavior) &
           registry.TryRegister(SortedDictionaryFactory.Default, behavior) &
           registry.TryRegister(SortedListFactory.Default, behavior) &
           registry.TryRegister(ConcurrentDictionaryFactory.Default, behavior) &
           registry.TryRegister(ConcurrentBagFactory.Default, behavior) &
           registry.TryRegister(ConcurrentQueueFactory.Default, behavior) &
           registry.TryRegister(ConcurrentStackFactory.Default, behavior) &
           registry.TryRegister(BlockingCollectionFactory.Default, behavior);

    public static IEnumerableFactoryRegistry RegisterAllDefaultFactories(this IEnumerableFactoryRegistry registry,
        RegistrationBehavior behavior = RegistrationBehavior.ThrowOnExisting)
    {
        registry.TryRegisterAllDefaultFactories(behavior);
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