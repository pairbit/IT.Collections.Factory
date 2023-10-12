namespace IT.Collections.Factory.Factories;

public class DictionaryFactory : DictionaryFactoryBase
{
    public static readonly DictionaryFactory Default = new();

    public override IDictionary<TKey, TValue> New<TKey, TValue>(int capacity) => new Dictionary<TKey, TValue>(capacity, null);
}