using System.Collections.Concurrent;

namespace IT.Collections.Factory.Factories;

public class ConcurrentDictionaryFactory : DictionaryFactoryBase
{
    public static readonly ConcurrentDictionaryFactory Default = new();

    public override IDictionary<TKey, TValue> New<TKey, TValue>(int capacity)
        => new ConcurrentDictionary<TKey, TValue>(Environment.ProcessorCount, capacity);
}