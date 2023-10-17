namespace IT.Collections.Factory;

public class EnumerableFactoryRegistry : EnumerableFactoryRegistry<Dictionary<Type, IEnumerableFactoryRegistrable>>
{
    public EnumerableFactoryRegistry(int capacity)
        : base(new Dictionary<Type, IEnumerableFactoryRegistrable>(capacity))
    {

    }

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
            try
            {
                _dictionary.Add(type, factory);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"Factory '{factory.GetType().FullName}' with type '{type.FullName}' is already registered", nameof(type));
            }
            return true;
        }
        throw new ArgumentOutOfRangeException(nameof(behavior));
    }
}