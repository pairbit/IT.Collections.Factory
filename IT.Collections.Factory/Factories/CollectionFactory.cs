using System.Collections.ObjectModel;

namespace IT.Collections.Factory.Factories;

public class CollectionFactory : IEnumerableFactory
{
    public static readonly CollectionFactory Default = new();

    public EnumerableType Type => EnumerableType.None;

    public IEnumerable<T> Empty<T>() => new Collection<T>();

    public IEnumerable<T> New<T>(int capacity)
    {
        if (capacity == 0) return new Collection<T>();

        return new Collection<T>(new List<T>(capacity));
    }

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return new Collection<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list = new List<T>(capacity);

        builder(list.Add, false);

        return new Collection<T>(list);
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return new Collection<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list = new List<T>(capacity);

        builder(list.Add, false, in state);

        return new Collection<T>(list);
    }
}