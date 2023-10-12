using System.Collections.Concurrent;

namespace IT.Collections.Factory.Factories;

public class ConcurrentQueueFactory : IEnumerableFactory
{
    public static readonly ConcurrentQueueFactory Default = new();

    public bool IsReadOnly => false;

    public IEnumerable<T> Empty<T>() => new ConcurrentQueue<T>();

    public IEnumerable<T> New<T>(int capacity) => new ConcurrentQueue<T>();

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return new ConcurrentQueue<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var queue = new ConcurrentQueue<T>();

        builder(queue.Enqueue, false);

        return queue;
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return new ConcurrentQueue<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var queue = new ConcurrentQueue<T>();

        builder(queue.Enqueue, false, in state);

        return queue;
    }
}