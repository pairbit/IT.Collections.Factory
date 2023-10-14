using System.Collections.ObjectModel;

namespace IT.Collections.Factory.Factories;

public class ObservableCollectionFactory : IEnumerableFactory
{
    public static readonly ObservableCollectionFactory Default = new();

    public EnumerableType Type => EnumerableType.None;

    public IEnumerable<T> Empty<T>() => new ObservableCollection<T>();

    public IEnumerable<T> New<T>(int capacity)
    {
        if (capacity == 0) return new ObservableCollection<T>();

        return new ObservableCollection<T>(new List<T>(capacity));
    }

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return new ObservableCollection<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list = new List<T>(capacity);

        builder(item => { list.Add(item); return true; });

        return new ObservableCollection<T>(list);
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return new ObservableCollection<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list = new List<T>(capacity);

        builder(item => { list.Add(item); return true; }, in state);

        return new ObservableCollection<T>(list);
    }
}