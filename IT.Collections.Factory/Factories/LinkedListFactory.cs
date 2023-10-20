namespace IT.Collections.Factory.Factories;

public class LinkedListFactory : ICollectionFactory, IReadOnlyCollectionFactory, IEquatable<LinkedListFactory>
{
    public static readonly LinkedListFactory Default = new();

    public virtual Type EnumerableType => typeof(LinkedList<>);

    public virtual EnumerableKind Kind => EnumerableKind.None;

    public virtual LinkedList<T> Empty<T>(in Comparers<T> comparers = default) =>
#if NET5_0_OR_GREATER
        new();
#else
        NewList(0, in comparers);
#endif

    public virtual LinkedList<T> New<T>(int capacity, in Comparers<T> comparers = default) =>
#if NET5_0_OR_GREATER
        new();
#else
        NewList(capacity, in comparers);
#endif

    public virtual LinkedList<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return
#if NET5_0_OR_GREATER
        new();
#else
        NewList(0, in comparers);
#endif
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list =
#if NET5_0_OR_GREATER
        new LinkedList<T>();
#else
        NewList(capacity, in comparers);
#endif

        builder(item => { list.AddLast(item); return true; });

        return list;
    }

    public virtual LinkedList<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return
#if NET5_0_OR_GREATER
        new();
#else
        NewList(0, in comparers);
#endif
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list =
#if NET5_0_OR_GREATER
        new LinkedList<T>();
#else
        NewList(capacity, in comparers);
#endif

        builder(item => { list.AddLast(item); return true; }, in state);

        return list;
    }

    public override int GetHashCode() => HashCode.Combine(GetType());

    public override bool Equals(object? obj) => Equals(obj as LinkedListFactory);

    public bool Equals(LinkedListFactory? other) => this == other || (other != null && other.GetType() == GetType());

#if !NET5_0_OR_GREATER
    protected virtual LinkedList<T> NewList<T>(int capacity, in Comparers<T> comparers) => new();
#endif

    ICollection<T> ICollectionFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    ICollection<T> ICollectionFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    ICollection<T> ICollectionFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    ICollection<T> ICollectionFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
    IEnumerable<T> IEnumerableFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
}