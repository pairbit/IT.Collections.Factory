namespace IT.Collections.Factory.Factories;

using Internal;

public class ReadOnlyLinkedListFactory :
#if NET5_0_OR_GREATER
    EnumerableFactory
#else
    IEnumerableFactory
#endif
{
    public static readonly ReadOnlyLinkedListFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.ReadOnly;

    public
#if NET5_0_OR_GREATER
        override
#endif
        IReadOnlyCollection<T> Empty<T>() => ReadOnlyCollection<T>.Empty;

    public
#if NET5_0_OR_GREATER
        override
#endif
        IReadOnlyCollection<T> New<T>(int capacity)
    {
        throw new NotSupportedException();
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        IReadOnlyCollection<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return ReadOnlyCollection<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list = new LinkedList<T>();

        builder(item => { list.AddLast(item); return true; });

        return new ReadOnlyCollection<T>(list);
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        IReadOnlyCollection<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return ReadOnlyCollection<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list = new LinkedList<T>();

        builder(item => { list.AddLast(item); return true; }, in state);

        return new ReadOnlyCollection<T>(list);
    }
#if !NET5_0_OR_GREATER
    IEnumerable<T> IEnumerableFactory.Empty<T>() => Empty<T>();
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity) => New<T>(capacity);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder) => New(capacity, builder);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state) => New(capacity, builder, in state);
#endif
}