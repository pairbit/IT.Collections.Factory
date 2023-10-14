namespace IT.Collections.Factory.Factories;

public class QueueFactory :
#if NET5_0_OR_GREATER
    EnumerableFactory
#else
    IEnumerableFactory
#endif
{
    public static readonly QueueFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.None;

    public
#if NET5_0_OR_GREATER
        override
#endif
        Queue<T> Empty<T>() => new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        Queue<T> New<T>(int capacity) => new(capacity);

    public
#if NET5_0_OR_GREATER
        override
#endif
        Queue<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return new();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var queue = new Queue<T>(capacity);

        builder(item => { queue.Enqueue(item); return true; });

        return queue;
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        Queue<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return new();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var queue = new Queue<T>(capacity);

        builder(item => { queue.Enqueue(item); return true; }, in state);

        return queue;
    }
#if !NET5_0_OR_GREATER
    IEnumerable<T> IEnumerableFactory.Empty<T>() => Empty<T>();
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity) => New<T>(capacity);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder) => New(capacity, builder);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state) => New(capacity, builder, in state);
#endif
}