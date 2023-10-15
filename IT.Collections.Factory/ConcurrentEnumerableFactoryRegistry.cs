using System.Collections.Concurrent;

namespace IT.Collections.Factory;

public class ConcurrentEnumerableFactoryRegistry : EnumerableFactoryRegistry<ConcurrentDictionary<Type, object>>
{
    public ConcurrentEnumerableFactoryRegistry(int capacity) 
        : base(new ConcurrentDictionary<Type, object>(Environment.ProcessorCount, capacity))
    {

    }

    public override void Clear() => _dictionary.Clear();

    public override bool TryRegister(Type type, object factory, RegistrationBehavior behavior)
    {
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