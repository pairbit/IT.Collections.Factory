namespace IT.Collections.Factory.Factories;

public class StackFactory : IReadOnlyCollectionFactory, IEquatable<StackFactory>
{
    public virtual Type EnumerableType => typeof(Stack<>);

    public virtual EnumerableKind Kind => EnumerableKind.Reverse;

    public virtual Stack<T> Empty<T>(in Comparers<T> comparers = default) =>
#if NET5_0_OR_GREATER
        new();
#else
        NewStack(0, in comparers);
#endif

    public virtual Stack<T> New<T>(int capacity, in Comparers<T> comparers = default) =>
#if NET5_0_OR_GREATER
        new(capacity);
#else
        NewStack(capacity, in comparers);
#endif

    public virtual Stack<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return
#if NET5_0_OR_GREATER
        new();
#else
        NewStack(0, in comparers);
#endif
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var stack =
#if NET5_0_OR_GREATER
        new Stack<T>(capacity);
#else
        NewStack(capacity, in comparers);
#endif

        builder(item => { stack.Push(item); return true; });

        return stack;
    }

    public virtual Stack<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return
#if NET5_0_OR_GREATER
        new();
#else
        NewStack(0, in comparers);
#endif
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var stack =
#if NET5_0_OR_GREATER
        new Stack<T>(capacity);
#else
        NewStack(capacity, in comparers);
#endif

        builder(item => { stack.Push(item); return true; }, in state);

        return stack;
    }

    public override int GetHashCode() => HashCode.Combine(GetType());

    public override bool Equals(object? obj) => Equals(obj as StackFactory);

    public bool Equals(StackFactory? other) => this == other || (other != null && other.GetType() == GetType());

#if !NET5_0_OR_GREATER
    protected virtual Stack<T> NewStack<T>(int capacity, in Comparers<T> comparers) => capacity == 0 ? new() : new(capacity);
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