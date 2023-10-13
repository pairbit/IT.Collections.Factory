#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public class ImmutableStackFactory : IEnumerableFactory
{
    public static readonly ImmutableStackFactory Default = new();

    public EnumerableType Type => EnumerableType.None;

    public IEnumerable<T> Empty<T>() => ImmutableStack<T>.Empty;

    public IEnumerable<T> New<T>(int capacity) => ImmutableStack<T>.Empty;

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return ImmutableStack<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var stack = ImmutableStack<T>.Empty;

        builder(item => stack = stack.Push(item), true);

        return stack;
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return ImmutableStack<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var stack = ImmutableStack<T>.Empty;

        builder(item => stack = stack.Push(item), true, in state);

        return stack;
    }
}

#endif