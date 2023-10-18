namespace IT.Collections.Factory.Factories;

using Internal;

public class ReadOnlyLinkedListFactory : IReadOnlyCollectionFactory
{
    public static readonly ReadOnlyLinkedListFactory Default = new();

    public virtual EnumerableKind Kind => EnumerableKind.ReadOnly;

    public virtual IReadOnlyCollection<T> Empty<T>(in Comparers<T> comparers = default) => ReadOnlyCollection<T>.Empty;

    public virtual IReadOnlyCollection<T> New<T>(int capacity, in Comparers<T> comparers = default)
    {
        throw new NotSupportedException();
    }

    public virtual IReadOnlyCollection<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ReadOnlyCollection<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list = new LinkedList<T>();

        builder(item => { list.AddLast(item); return true; });

        return new ReadOnlyCollection<T>(list);
    }

    public virtual IReadOnlyCollection<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ReadOnlyCollection<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list = new LinkedList<T>();

        builder(item => { list.AddLast(item); return true; }, in state);

        return new ReadOnlyCollection<T>(list);
    }

    IEnumerable<T> IEnumerableFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
}