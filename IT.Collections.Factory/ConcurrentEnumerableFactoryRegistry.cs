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

    public override bool TryRegisterFactory(Type type, IEnumerableFactoryRegistrable factory, RegistrationBehavior behavior)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (factory == null) throw new ArgumentNullException(nameof(factory));

        if (behavior == RegistrationBehavior.None) return _dictionary.TryAdd(type, factory);
        if (behavior == RegistrationBehavior.OverwriteExisting)
        {
            _dictionary[type] = factory;
            return true;
        }
        if (behavior == RegistrationBehavior.ThrowOnExisting)
        {
            if (!_dictionary.TryAdd(type, factory)) throw Ex.FactoryTypeRegistered(factory.GetType(), type, nameof(type));
            return true;
        }
        throw new ArgumentOutOfRangeException(nameof(behavior));
    }
}