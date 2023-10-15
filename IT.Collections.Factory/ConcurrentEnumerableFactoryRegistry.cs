namespace IT.Collections.Factory;

public class ConcurrentEnumerableFactoryRegistry : EnumerableFactoryRegistry<ConcurrentDictionary<Type, IEnumerableFactoryRegistrable>>
{
    public ConcurrentEnumerableFactoryRegistry(int concurrencyLevel, int capacity)
        : base(new ConcurrentDictionary<Type, IEnumerableFactoryRegistrable>(concurrencyLevel == -1 ? Environment.ProcessorCount : concurrencyLevel, capacity))
    {

    }

    public override void Clear() => _dictionary.Clear();

    public override bool TryRegister(Type type, IEnumerableFactoryRegistrable factory, RegistrationBehavior behavior)
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
            if (!_dictionary.TryAdd(type, factory)) throw new ArgumentException("Duplicate key", nameof(type));
            return true;
        }
        throw new ArgumentOutOfRangeException(nameof(behavior));
    }
}