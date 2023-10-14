namespace IT.Collections.Factory.Factories;

public class StackFactory :
#if NET5_0_OR_GREATER
    EnumerableFactory
#else
    IEnumerableFactory
#endif
{
    public static readonly StackFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.Reverse;

    public
#if NET5_0_OR_GREATER
        override
#endif
        Stack<T> Empty<T>() => new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        Stack<T> New<T>(int capacity) => new(capacity);

    public
#if NET5_0_OR_GREATER
        override
#endif
        Stack<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return new();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var stack = new Stack<T>(capacity);

        builder(item => { stack.Push(item); return true; });

        return stack;
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        Stack<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return new();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var stack = new Stack<T>(capacity);

        builder(item => { stack.Push(item); return true; }, in state);

        return stack;
    }
#if !NET5_0_OR_GREATER
    IEnumerable<T> IEnumerableFactory.Empty<T>() => Empty<T>();
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity) => New<T>(capacity);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder) => New(capacity, builder);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state) => New(capacity, builder, in state);
#endif
}