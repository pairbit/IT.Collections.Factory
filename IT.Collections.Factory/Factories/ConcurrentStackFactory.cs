namespace IT.Collections.Factory.Factories;

public class ConcurrentStackFactory : IProducerConsumerCollectionFactory, IReadOnlyCollectionFactory
{
    public static readonly ConcurrentStackFactory Default = new();

    public virtual EnumerableKind Kind => EnumerableKind.Reverse | EnumerableKind.ThreadSafe;

    public virtual ConcurrentStack<T> Empty<T>(in Comparers<T> comparers = default) => new();

    public virtual ConcurrentStack<T> New<T>(int capacity, in Comparers<T> comparers = default) => new();

    public virtual ConcurrentStack<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return new();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var stack = new ConcurrentStack<T>();

        builder(((IProducerConsumerCollection<T>)stack).TryAdd);

        return stack;
    }

    public virtual ConcurrentStack<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return new();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var stack = new ConcurrentStack<T>();

        builder(((IProducerConsumerCollection<T>)stack).TryAdd, in state);

        return stack;
    }

    IProducerConsumerCollection<T> IProducerConsumerCollectionFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IProducerConsumerCollection<T> IProducerConsumerCollectionFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IProducerConsumerCollection<T> IProducerConsumerCollectionFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IProducerConsumerCollection<T> IProducerConsumerCollectionFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
    IEnumerable<T> IEnumerableFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
}