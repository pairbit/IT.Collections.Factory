namespace IT.Collections.Factory;

using Internal;

internal sealed class GlobalEnumerableFactoryRegistry : IEnumerableFactoryRegistry
{
    private static readonly ConcurrentDictionary<Type, IEnumerableFactoryRegistrable> _dictionary 
        = new(Environment.ProcessorCount, EnumerableFactoryRegistry.CapacityDefault);

    public static readonly GlobalEnumerableFactoryRegistry Default = new();

    static class Check<TFactory>
    {
        public static bool _registered;
    }

    static class Cache<TFactory> where TFactory : IEnumerableFactoryRegistrable
    {
        public static TFactory? _factory;

        static Cache()
        {
            if (Check<TFactory>._registered) return;

            var factoryProxy = (TFactory?)_dictionary.TryGetFactoryProxy(typeof(TFactory));
            if (factoryProxy != null)
            {
                _factory = factoryProxy;
                _dictionary[typeof(TFactory)] = factoryProxy;
                Check<TFactory>._registered = true;
            }
        }

        public static void Register(TFactory factory, Type type)
        {
            _factory = factory;
            _dictionary[typeof(TFactory)] = factory;
            //TODO: EnumerableType как быть? Регистрировать отдельно? как проверять?
            _dictionary[type] = factory;
        }
    }

    private GlobalEnumerableFactoryRegistry() { }

    public bool IsRegistered<TFactory>() where TFactory : IEnumerableFactoryRegistrable 
        => Check<TFactory>._registered;

    public void Clear() => _dictionary.Clear();

    public bool TryRegisterFactory<TFactory>(TFactory factory, RegistrationBehavior behavior) where TFactory : IEnumerableFactoryRegistrable
    {
        if (factory == null) throw new ArgumentNullException(nameof(factory));
        //if (!behavior.IsValid()) throw Ex.BehaviorInvalid(behavior, nameof(behavior));

        var enumerableType = factory.EnumerableType ?? throw Ex.EnumerableTypeIsNull(typeof(TFactory), nameof(factory));
        if (!enumerableType.IsAssignableToEnumerable()) throw Ex.EnumerableTypeNotEnumerable(typeof(TFactory), enumerableType, nameof(factory));
        if (!CacheFactory<TFactory>.IsValid) throw new ArgumentException(CacheFactory<TFactory>.Error);

        //TODO: EnumerableType как быть? Регистрировать отдельно? как проверять?
        var returnType = CacheFactory<TFactory>.ReturnType!;
        if (enumerableType != returnType && !enumerableType.IsAssignableToDefinition(returnType))
            throw Ex.EnumerableTypeNotInheritedFromReturnType(typeof(TFactory), enumerableType, returnType, nameof(factory));

        if (behavior == RegistrationBehavior.None)
        {
            if (Check<TFactory>._registered) return false;

            Check<TFactory>._registered = true;
            Cache<TFactory>.Register(factory, returnType);
            return true;
        }
        if (behavior == RegistrationBehavior.OverwriteExisting)
        {
            Check<TFactory>._registered = true;
            Cache<TFactory>.Register(factory, returnType);
            return true;
        }
        if (behavior == RegistrationBehavior.ThrowOnExisting)
        {
            if (Check<TFactory>._registered) throw Ex.FactoryTypeRegistered(factory.GetType(), typeof(TFactory));

            Check<TFactory>._registered = true;
            Cache<TFactory>.Register(factory, returnType);
            return true;
        }
        throw new ArgumentOutOfRangeException(nameof(behavior));
    }

    public TFactory? TryGetFactory<TFactory>() where TFactory : IEnumerableFactoryRegistrable
        => Cache<TFactory>._factory;

    #region IReadOnlyDictionary

    public IEnumerableFactoryRegistrable this[Type key] => _dictionary[key];

    public IEnumerable<Type> Keys => _dictionary.Keys;

    public IEnumerable<IEnumerableFactoryRegistrable> Values => _dictionary.Values;

    public int Count => _dictionary.Count;

    public bool ContainsKey(Type key) => _dictionary.ContainsKey(key);

    public IEnumerator<KeyValuePair<Type, IEnumerableFactoryRegistrable>> GetEnumerator() => _dictionary.GetEnumerator();

    public bool TryGetValue(Type key,
#if NETCOREAPP3_1_OR_GREATER
        [System.Diagnostics.CodeAnalysis.MaybeNullWhen(false)]
#endif
        out IEnumerableFactoryRegistrable value) => _dictionary.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_dictionary).GetEnumerator();

    #endregion IReadOnlyDictionary
}