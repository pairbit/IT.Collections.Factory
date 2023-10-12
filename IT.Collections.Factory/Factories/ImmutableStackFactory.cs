#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public class ImmutableStackFactory : IEnumerableFactory
{
    public static readonly ImmutableStackFactory Default = new();

    public bool IsReadOnly => false;

    public IEnumerable<T> Empty<T>() => ImmutableStack<T>.Empty;

    public IEnumerable<T> New<T>(int capacity)
    {
        if (capacity == 0) return ImmutableStack<T>.Empty;

        var array = new T[capacity];

        var stack = ImmutableStack<T>.Empty;

        for (int i = array.Length - 1; i >= 0; i--) stack = stack.Push(array[i]);

        return stack;
    }

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return ImmutableStack<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var array = new T[capacity];

        builder(array);

        var stack = ImmutableStack<T>.Empty;

        for (int i = array.Length - 1; i >= 0; i--) stack = stack.Push(array[i]);

        return stack;
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return ImmutableStack<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var array = new T[capacity];

        builder(array, in state);

        var stack = ImmutableStack<T>.Empty;

        for (int i = array.Length - 1; i >= 0; i--) stack = stack.Push(array[i]);

        return stack;
    }
}

#endif