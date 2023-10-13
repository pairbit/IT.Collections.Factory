using System.Collections.Concurrent;

namespace IT.Collections.Factory.Factories;

public class ConcurrentBagFactory : IEnumerableFactory
{
    public static readonly ConcurrentBagFactory Default = new();

    public EnumerableType Type => EnumerableType.Reverse;

    public IEnumerable<T> Empty<T>() => new ConcurrentBag<T>();

    public IEnumerable<T> New<T>(int capacity) => new ConcurrentBag<T>();

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return new ConcurrentBag<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var bag = new ConcurrentBag<T>();

        builder(bag.Add, true);

        return bag;
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return new ConcurrentBag<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var bag = new ConcurrentBag<T>();

        builder(bag.Add, true, in state);

        return bag;
    }
}