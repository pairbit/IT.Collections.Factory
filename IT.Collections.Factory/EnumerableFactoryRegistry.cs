namespace IT.Collections.Factory;

public class EnumerableFactoryRegistry : EnumerableFactoryRegistry<Dictionary<Type, IEnumerableFactoryRegistrable>>
{
    public EnumerableFactoryRegistry(int capacity)
        : base(new Dictionary<Type, IEnumerableFactoryRegistrable>(capacity))
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
            _dictionary.Add(type, factory);
            return true;
        }
        throw new ArgumentOutOfRangeException(nameof(behavior));
    }
}