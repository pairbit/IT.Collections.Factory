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
        ReadOnlyCollection<T> Empty<T>() => Cache<T>.Empty;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ReadOnlyCollection<T> New<T>(int capacity)
    {
        throw new NotSupportedException();
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        ReadOnlyCollection<T> New<T>(int capacity, EnumerableBuilder<T> builder)
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
        ReadOnlyCollection<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
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
    IEnumerable<T> IEnumerableFactory.Empty<T>() => Empty<T>();
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity) => New<T>(capacity);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder) => New(capacity, builder);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state) => New(capacity, builder, in state);
#endif
}