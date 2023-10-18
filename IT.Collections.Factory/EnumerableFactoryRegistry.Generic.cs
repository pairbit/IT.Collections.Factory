﻿namespace IT.Collections.Factory;

using Internal;

public abstract class EnumerableFactoryRegistry<TDictionary> : IEnumerableFactoryRegistry
    where TDictionary : IReadOnlyDictionary<Type, IEnumerableFactoryRegistrable>
{
    protected readonly TDictionary _dictionary;

    protected EnumerableFactoryRegistry(TDictionary dictionary)
    {
        _dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
    }

    public abstract void Clear();

    public abstract bool TryRegisterFactory(Type type, IEnumerableFactoryRegistrable factory, RegistrationBehavior behavior);

    public virtual bool TryRegisterFactory<TFactory>(TFactory factory, RegistrationBehavior behavior) where TFactory : IEnumerableFactoryRegistrable
    {
        if (!CacheFactory<TFactory>.IsValid) throw new ArgumentException(CacheFactory<TFactory>.Error);

        var factoryType = typeof(TFactory);

        //TODO: EnumerableTypeDefinition как быть? Регистрировать отдельно? как проверять?
        var enumerableTypeDefinition = CacheFactory<TFactory>.EnumerableTypeDefinition!;

        return TryRegisterFactory(factoryType, factory, behavior) &
               TryRegisterFactory(enumerableTypeDefinition, factory, behavior);
    }

    public virtual TFactory? TryGetFactory<TFactory>() where TFactory : IEnumerableFactoryRegistrable
    {
        var factoryType = typeof(TFactory);
        if (_dictionary.TryGetValue(factoryType, out var factory)) return (TFactory?)factory;

        factory = this.TryGetFactoryProxy(factoryType);
        if (factory != null)
        {
            //Пробуем зарегистрировать прокси фабрику
            if (!TryRegisterFactory(factoryType, factory, RegistrationBehavior.None))
            {
                //Кто-то успел зарегистрировать фабрику, получаем ее
                factory = _dictionary[factoryType];
            }
            return (TFactory?)factory;
        }
        return default;
    }

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