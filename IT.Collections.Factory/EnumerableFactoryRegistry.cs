﻿namespace IT.Collections.Factory;

public class EnumerableFactoryRegistry : EnumerableFactoryRegistry<Dictionary<Type, object>>
{
    public EnumerableFactoryRegistry(int capacity)
        : base(new Dictionary<Type, object>(capacity))
    {

    }

    public override void Clear() => _dictionary.Clear();

    public override bool Register(Type type, object factory, bool overwrite)
    {
        if (overwrite)
        {
            //var exists = _dictionary.ContainsKey(type);

            _dictionary[type] = factory;

            return true;
        }

        return _dictionary.TryAdd(type, factory);
    }
}