using IT.Collections.Factory.Internal;

namespace IT.Collections.Factory;

internal sealed class GlobalEnumerableFactoryRegistry : IEnumerableFactoryRegistry
{
    private static readonly ConcurrentDictionary<Type, IEnumerableFactoryRegistrable> _dictionary 
        = new(Environment.ProcessorCount, 100);

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

        public static void Register(TFactory factory)
        {
            _factory = factory;
            _dictionary[typeof(TFactory)] = factory;
            //TODO: EnumerableTypeDefinition как быть? Регистрировать отдельно? как проверять?
            _dictionary[CacheFactory<TFactory>.EnumerableTypeDefinition!] = factory;
        }
    }

    private GlobalEnumerableFactoryRegistry() { }

    public bool IsRegistered<TFactory>() where TFactory : IEnumerableFactoryRegistrable 
        => Check<TFactory>._registered;

    public void Clear() => _dictionary.Clear();

    public bool TryRegisterFactory<TFactory>(TFactory factory, RegistrationBehavior behavior) where TFactory : IEnumerableFactoryRegistrable
    {
        if (factory == null) throw new ArgumentNullException(nameof(factory));
        if (!CacheFactory<TFactory>.IsValid) throw new ArgumentException(CacheFactory<TFactory>.Error);

        if (behavior == RegistrationBehavior.None)
        {
            if (Check<TFactory>._registered) return false;

            Check<TFactory>._registered = true;
            Cache<TFactory>.Register(factory);
            return true;
        }
        if (behavior == RegistrationBehavior.OverwriteExisting)
        {
            Check<TFactory>._registered = true;
            Cache<TFactory>.Register(factory);
            return true;
        }
        if (behavior == RegistrationBehavior.ThrowOnExisting)
        {
            if (Check<TFactory>._registered) throw new ArgumentException($"Factory '{factory.GetType().FullName}' with type '{typeof(TFactory).FullName}' is already registered");

            Check<TFactory>._registered = true;
            Cache<TFactory>.Register(factory);
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