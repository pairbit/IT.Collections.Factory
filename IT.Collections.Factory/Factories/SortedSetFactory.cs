namespace IT.Collections.Factory.Factories;

public class SortedSetFactory :
#if NET5_0_OR_GREATER
    EnumerableFactory
#else
    IEnumerableFactory
#endif
{
    public static readonly SortedSetFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.Ordered | EnumerableType.Unique;

    public
#if NET5_0_OR_GREATER
        override
#endif
        SortedSet<T> Empty<T>() => new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        SortedSet<T> New<T>(int capacity) => new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        SortedSet<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return new();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var sortedSet = new SortedSet<T>();

        builder(sortedSet.Add);

        return sortedSet;
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        SortedSet<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return new();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var sortedSet = new SortedSet<T>();

        builder(sortedSet.Add, in state);

        return sortedSet;
    }
#if !NET5_0_OR_GREATER
    IEnumerable<T> IEnumerableFactory.Empty<T>() => Empty<T>();
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity) => New<T>(capacity);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder) => New(capacity, builder);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state) => New(capacity, builder, in state);
#endif
}