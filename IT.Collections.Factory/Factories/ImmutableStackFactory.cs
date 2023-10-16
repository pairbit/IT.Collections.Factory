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
        ImmutableStack<T> Empty<T>(in Comparers<T> comparers = default) => ImmutableStack<T>.Empty;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableStack<T> New<T>(int capacity, in Comparers<T> comparers = default) => ImmutableStack<T>.Empty;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableStack<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
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
        ImmutableStack<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ImmutableStack<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var stack = ImmutableStack<T>.Empty;

        builder(item => { stack = stack.Push(item); return true; }, in state);

        return stack;
    }
#if !NET5_0_OR_GREATER
    IEnumerable<T> IEnumerableFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
#endif
}

#endif