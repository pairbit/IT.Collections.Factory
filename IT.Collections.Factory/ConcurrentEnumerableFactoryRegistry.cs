using System.Collections.Concurrent;

namespace IT.Collections.Factory;

public class ConcurrentEnumerableFactoryRegistry : EnumerableFactoryRegistry<ConcurrentDictionary<Type, object>>
{
    public ConcurrentEnumerableFactoryRegistry(int capacity) 
        : base(new ConcurrentDictionary<Type, object>(Environment.ProcessorCount, capacity))
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