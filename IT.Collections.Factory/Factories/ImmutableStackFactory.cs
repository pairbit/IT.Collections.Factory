#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public class ImmutableStackFactory :
#if NET5_0_OR_GREATER
    EnumerableFactory
#else
    IEnumerableFactory
#endif
{
    public static readonly ImmutableStackFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.Reverse;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableStack<T> Empty<T>() => ImmutableStack<T>.Empty;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableStack<T> New<T>(int capacity) => ImmutableStack<T>.Empty;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableStack<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return ImmutableStack<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var stack = ImmutableStack<T>.Empty;

        builder(item => { stack = stack.Push(item); return true; });

        return stack;
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableStack<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return ImmutableStack<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var stack = ImmutableStack<T>.Empty;

        builder(item => { stack = stack.Push(item); return true; }, in state);

        return stack;
    }
#if !NET5_0_OR_GREATER
    IEnumerable<T> IEnumerableFactory.Empty<T>() => Empty<T>();
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity) => New<T>(capacity);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder) => New(capacity, builder);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state) => New(capacity, builder, in state);
#endif
}

#endif