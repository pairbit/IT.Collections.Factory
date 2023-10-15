namespace IT.Collections.Factory;

using Factories;
using Internal;

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
           registry.TryRegister(ConcurrentBagFactory.Default, behavior);

    public static bool TryRegisterFactoriesDefault(this IEnumerableFactoryRegistry registry, RegistrationBehavior behavior)
        => registry.TryRegister(typeof(List<>), ListFactory.Default, behavior) &
           registry.TryRegister(typeof(IList<>), ListFactory.Default, behavior) &
           registry.TryRegister(typeof(LinkedList<>), LinkedListFactory.Default, behavior) &
           registry.TryRegister(typeof(ICollection<>), LinkedListFactory.Default, behavior) &
           registry.TryRegister(typeof(HashSet<>), HashSetFactory.Default, behavior) &
           registry.TryRegister(typeof(ISet<>), HashSetFactory.Default, behavior) &
           registry.TryRegister(SortedSetFactory.Default, behavior);
}
/*
 
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
 */