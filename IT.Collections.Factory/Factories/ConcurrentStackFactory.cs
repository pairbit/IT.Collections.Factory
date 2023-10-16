namespace IT.Collections.Factory.Factories;

public class ConcurrentStackFactory :
#if NET5_0_OR_GREATER
    EnumerableFactory
#else
    IEnumerableFactory
#endif
{
    public static readonly ConcurrentStackFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.Reverse;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ConcurrentStack<T> Empty<T>(in Comparers<T> comparers = default) => new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        ConcurrentStack<T> New<T>(int capacity, in Comparers<T> comparers = default) => new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        ConcurrentStack<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return new();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var stack = new ConcurrentStack<T>();

        builder(((IProducerConsumerCollection<T>)stack).TryAdd);

        return stack;
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        ConcurrentStack<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return new();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var stack = new ConcurrentStack<T>();

        builder(((IProducerConsumerCollection<T>)stack).TryAdd, in state);

        return stack;
    }
#if !NET5_0_OR_GREATER
    IEnumerable<T> IEnumerableFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
#endif
}