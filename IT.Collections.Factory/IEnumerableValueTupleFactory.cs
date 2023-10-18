#if !NET461

namespace IT.Collections.Factory;

internal interface IEnumerableValueTupleFactory : IEnumerableFactoryRegistrable
{
    IEnumerable<ValueTuple<TKey, TValue>> Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers = default);

    IEnumerable<ValueTuple<TKey, TValue>> New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers = default);

    IEnumerable<ValueTuple<TKey, TValue>> New<TKey, TValue>(int capacity, EnumerableBuilder<ValueTuple<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers = default);

    IEnumerable<ValueTuple<TKey, TValue>> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<ValueTuple<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers = default);
}

#endif