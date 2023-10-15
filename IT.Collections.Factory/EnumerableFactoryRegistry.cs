namespace IT.Collections.Factory;

public class EnumerableFactoryRegistry : EnumerableFactoryRegistry<Dictionary<Type, object>>
{
    public EnumerableFactoryRegistry(int capacity)
        : base(new Dictionary<Type, object>(capacity))
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
            _dictionary.Add(type, factory);
            return true;
        }
        throw new ArgumentOutOfRangeException(nameof(behavior));
    }
}