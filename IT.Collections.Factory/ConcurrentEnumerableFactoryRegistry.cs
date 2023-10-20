namespace IT.Collections.Factory;

using Internal;

public class ConcurrentEnumerableFactoryRegistry : EnumerableFactoryRegistry<ConcurrentDictionary<Type, IEnumerableFactoryRegistrable>>
{
    public ConcurrentEnumerableFactoryRegistry() : this(-1, -1) { }

    public ConcurrentEnumerableFactoryRegistry(int concurrencyLevel, int capacity)
        : base(new ConcurrentDictionary<Type, IEnumerableFactoryRegistrable>(
            concurrencyLevel == -1 ? Environment.ProcessorCount : concurrencyLevel,
            capacity == -1 ? EnumerableFactoryRegistry.CapacityDefault : capacity))
    { }

    public override void Clear() => _dictionary.Clear();

    protected override bool TryRegisterFactoryInternal(Type type, IEnumerableFactoryRegistrable factory, RegistrationBehavior behavior)
    {
        if (behavior == RegistrationBehavior.None)
        {
            //_dictionary.GetOrAdd
            if (_dictionary.TryAdd(type, factory)) return true;
            if (_dictionary.TryGetValue(type, out var factoryRegistered) && factoryRegistered.Equals(factory)) return true;

            return false;
        }
        if (behavior == RegistrationBehavior.OverwriteExisting)
        {
            _dictionary[type] = factory;
            return true;
        }
        if (behavior == RegistrationBehavior.ThrowOnExisting)
        {
            if (_dictionary.TryAdd(type, factory)) return true;
            if (_dictionary.TryGetValue(type, out var factoryRegistered) && factoryRegistered.Equals(factory)) return true;

            throw Ex.FactoryTypeRegistered(factory.GetType(), type, nameof(type));
        }
        throw Ex.BehaviorInvalid(behavior, nameof(behavior));
    }
}