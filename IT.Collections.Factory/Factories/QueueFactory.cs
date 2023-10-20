namespace IT.Collections.Factory.Factories;

public class QueueFactory : IReadOnlyCollectionFactory, IEquatable<QueueFactory>
{
    public virtual Type EnumerableType => typeof(Queue<>);

    public virtual EnumerableKind Kind => EnumerableKind.None;

    public virtual Queue<T> Empty<T>(in Comparers<T> comparers = default) =>
#if NET5_0_OR_GREATER
        new();
#else
        NewQueue(0, in comparers);
#endif

    public virtual Queue<T> New<T>(int capacity, in Comparers<T> comparers = default) =>
#if NET5_0_OR_GREATER
        new(capacity);
#else
        NewQueue(capacity, in comparers);
#endif

    public virtual Queue<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return
#if NET5_0_OR_GREATER
        new();
#else
        NewQueue(0, in comparers);
#endif
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var queue =
#if NET5_0_OR_GREATER
        new Queue<T>(capacity);
#else
        NewQueue(capacity, in comparers);
#endif

        builder(item => { queue.Enqueue(item); return true; });

        return queue;
    }

    public virtual Queue<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return
#if NET5_0_OR_GREATER
        new();
#else
        NewQueue(0, in comparers);
#endif
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var queue =
#if NET5_0_OR_GREATER
        new Queue<T>(capacity);
#else
        NewQueue(capacity, in comparers);
#endif

        builder(item => { queue.Enqueue(item); return true; }, in state);

        return queue;
    }

    public override int GetHashCode() => HashCode.Combine(GetType());

    public override bool Equals(object? obj) => Equals(obj as QueueFactory);

    public bool Equals(QueueFactory? other) => this == other || (other != null && other.GetType() == GetType());

#if !NET5_0_OR_GREATER
    protected virtual Queue<T> NewQueue<T>(int capacity, in Comparers<T> comparers) => capacity == 0 ? new() : new(capacity);
#endif

    IReadOnlyCollection<T> IReadOnlyCollectionFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
    IEnumerable<T> IEnumerableFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
}