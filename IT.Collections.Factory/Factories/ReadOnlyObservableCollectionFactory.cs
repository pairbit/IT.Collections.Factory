using System.Collections.ObjectModel;

namespace IT.Collections.Factory.Factories;

public class ReadOnlyObservableCollectionFactory : IEnumerableFactory
{
    public static readonly ReadOnlyObservableCollectionFactory Default = new();

    public bool IsReadOnly => true;

    public IEnumerable<T> Empty<T>() => Cache<T>.Empty;

    public IEnumerable<T> New<T>(int capacity)
    {
        throw new NotSupportedException();
    }

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return Cache<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list = new List<T>(capacity);

        builder(list);

        return new ReadOnlyObservableCollection<T>(new ObservableCollection<T>(list));
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return Cache<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list = new List<T>(capacity);

        builder(list, in state);

        return new ReadOnlyObservableCollection<T>(new ObservableCollection<T>(list));
    }

    static class Cache<T>
    {
        public readonly static ReadOnlyObservableCollection<T> Empty = new(new ObservableCollection<T>());
    }
}