namespace IT.Collections.Factory.Factories;

public class QueueFactory : IEnumerableFactory
{
    public static readonly QueueFactory Default = new();

    public bool IsReadOnly => false;

    public IEnumerable<T> Empty<T>() => new Queue<T>();

    public IEnumerable<T> New<T>(int capacity) => new Queue<T>(capacity);

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return new Queue<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var queue = new Queue<T>(capacity);

        builder(queue.Enqueue, false);

        return queue;
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return new Queue<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var queue = new Queue<T>(capacity);

        builder(queue.Enqueue, false, in state);

        return queue;
    }
}