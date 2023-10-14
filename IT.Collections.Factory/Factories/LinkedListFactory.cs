namespace IT.Collections.Factory.Factories;

public class LinkedListFactory :
#if NET5_0_OR_GREATER
    EnumerableFactory
#else
    IEnumerableFactory
#endif
{
    public static readonly LinkedListFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.None;

    public
#if NET5_0_OR_GREATER
        override
#endif
        LinkedList<T> Empty<T>() => new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        LinkedList<T> New<T>(int capacity) => new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        LinkedList<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return new();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list = new LinkedList<T>();

        builder(item => { list.AddLast(item); return true; });

        return list;
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        LinkedList<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return new();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list = new LinkedList<T>();

        builder(item => { list.AddLast(item); return true; }, in state);

        return list;
    }
#if !NET5_0_OR_GREATER
    IEnumerable<T> IEnumerableFactory.Empty<T>() => Empty<T>();
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity) => New<T>(capacity);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder) => New(capacity, builder);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state) => New(capacity, builder, in state);
#endif
}