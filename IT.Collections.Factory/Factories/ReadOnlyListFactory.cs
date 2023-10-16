using System.Collections.ObjectModel;

namespace IT.Collections.Factory.Factories;

public class ReadOnlyListFactory :
#if NET5_0_OR_GREATER
    EnumerableFactory
#else
    IEnumerableFactory
#endif
{
    public static readonly ReadOnlyListFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.ReadOnly;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ReadOnlyCollection<T> Empty<T>(in Comparers<T> comparers = default) => Cache<T>.Empty;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ReadOnlyCollection<T> New<T>(int capacity, in Comparers<T> comparers = default)
    {
        throw new NotSupportedException();
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        ReadOnlyCollection<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return Cache<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list = new List<T>(capacity);

        builder(item => { list.Add(item); return true; });

        return new(list);
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        ReadOnlyCollection<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return Cache<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list = new List<T>(capacity);

        builder(item => { list.Add(item); return true; }, in state);

        return new(list);
    }

    static class Cache<T>
    {
        public readonly static ReadOnlyCollection<T> Empty = new(new List<T>());
    }
#if !NET5_0_OR_GREATER
    IEnumerable<T> IEnumerableFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
#endif
}