namespace IT.Collections.Factory;

using Internal;

public class EnumerableFactoryRegistry : EnumerableFactoryRegistry<Dictionary<Type, IEnumerableFactoryRegistrable>>
{
    public static readonly int CapacityDefault = 100;

    public static IEnumerableFactoryRegistry Global => GlobalEnumerableFactoryRegistry.Default;

    public EnumerableFactoryRegistry() : this(-1) { }

    public EnumerableFactoryRegistry(int capacity)
        : base(new Dictionary<Type, IEnumerableFactoryRegistrable>(capacity == -1 ? CapacityDefault : capacity)) { }

    public override void Clear() => _dictionary.Clear();

    public override bool TryRegisterFactory(Type type, IEnumerableFactoryRegistrable factory, RegistrationBehavior behavior)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (factory == null) throw new ArgumentNullException(nameof(factory));

        if (behavior == RegistrationBehavior.None)
        {
#if NETSTANDARD2_0 || NET461_OR_GREATER
            if (_dictionary.TryGetValue(type, out var factoryRegistered))
                return factoryRegistered.Equals(factory);

            _dictionary.Add(type, factory);
            return true;
#else
            if (_dictionary.TryAdd(type, factory)) return true;
            if (_dictionary.TryGetValue(type, out var factoryRegistered) && factoryRegistered.Equals(factory)) return true;
            return false;
#endif
        }
        if (behavior == RegistrationBehavior.OverwriteExisting)
        {
            _dictionary[type] = factory;
            return true;
        }
        if (behavior == RegistrationBehavior.ThrowOnExisting)
        {
#if NETSTANDARD2_0 || NET461_OR_GREATER
            try
            {
                _dictionary.Add(type, factory);
            }
            catch (ArgumentException)
            {
                if (_dictionary.TryGetValue(type, out var factoryRegistered) && factoryRegistered.Equals(factory)) return true;

                throw Ex.FactoryTypeRegistered(factory.GetType(), type, nameof(type));
            }
            return true;
#else
            if (_dictionary.TryAdd(type, factory)) return true;
            if (_dictionary.TryGetValue(type, out var factoryRegistered) && factoryRegistered.Equals(factory)) return true;

            throw Ex.FactoryTypeRegistered(factory.GetType(), type, nameof(type));
#endif
        }
        throw Ex.BehaviorInvalid(behavior, nameof(behavior));
    }
}