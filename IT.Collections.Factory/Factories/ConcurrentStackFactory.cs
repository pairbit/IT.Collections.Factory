using System.Collections.Concurrent;

namespace IT.Collections.Factory.Factories;

public class ConcurrentStackFactory : IEnumerableFactory
{
    public static readonly ConcurrentStackFactory Default = new();

    public bool IsReadOnly => false;

    public IEnumerable<T> Empty<T>() => new ConcurrentStack<T>();

    public IEnumerable<T> New<T>(int capacity) => new ConcurrentStack<T>();

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return new ConcurrentStack<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var stack = new ConcurrentStack<T>();

        builder(stack.Push, true);

        return stack;
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return new ConcurrentStack<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var stack = new ConcurrentStack<T>();

        builder(stack.Push, true, in state);

        return stack;
    }
}