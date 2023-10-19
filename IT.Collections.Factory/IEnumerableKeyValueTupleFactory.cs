#if !NET461

namespace IT.Collections.Factory;

internal interface IEnumerableKeyValueTupleFactory : IEnumerableFactoryRegistrable
{
    IEnumerable<(TKey, TValue)> Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers = default);

    IEnumerable<(TKey, TValue)> New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers = default);

    IEnumerable<(TKey, TValue)> New<TKey, TValue>(int capacity, EnumerableBuilder<(TKey, TValue)> builder, in Comparers<TKey, TValue> comparers = default);

    IEnumerable<(TKey, TValue)> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<(TKey, TValue), TState> builder, in TState state, in Comparers<TKey, TValue> comparers = default);
}

#endif