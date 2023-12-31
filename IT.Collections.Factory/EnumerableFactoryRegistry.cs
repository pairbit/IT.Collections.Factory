﻿namespace IT.Collections.Factory;

using Internal;

public class EnumerableFactoryRegistry : EnumerableFactoryRegistry<Dictionary<Type, IEnumerableFactoryRegistrable>>
{
    public static readonly int CapacityDefault = 100;

    internal static IEnumerableFactoryRegistry Global => GlobalEnumerableFactoryRegistry.Default;

    public EnumerableFactoryRegistry() : this(-1) { }

    public EnumerableFactoryRegistry(int capacity)
        : base(new Dictionary<Type, IEnumerableFactoryRegistrable>(capacity == -1 ? CapacityDefault : capacity)) { }

    public override void Clear() => _dictionary.Clear();

    protected override bool TryRegisterFactoryInternal(Type type, IEnumerableFactoryRegistrable factory, RegistrationBehavior behavior)
    {
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
            if (_dictionary.TryGetValue(type, out var factoryRegistered))
                return factoryRegistered.Equals(factory) ? true : throw Ex.FactoryTypeRegistered(factory.GetType(), type, nameof(type));

            _dictionary.Add(type, factory);
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