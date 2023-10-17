using System.Collections.ObjectModel;

namespace IT.Collections.Factory.Factories;

public class ObservableCollectionFactory : CollectionFactory
{
    public static new readonly ObservableCollectionFactory Default = new();

    public override EnumerableType Type => EnumerableType.None;

    public
#if NET5_0_OR_GREATER
        override
#else
        new
#endif
        ObservableCollection<T> Empty<T>(in Comparers<T> comparers = default) => new();

    public
#if NET5_0_OR_GREATER
        override
#else
        new
#endif
        ObservableCollection<T> New<T>(int capacity, in Comparers<T> comparers = default) => new();

    public
#if NET5_0_OR_GREATER
        override
#else
        new
#endif
        ObservableCollection<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return new();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var collection = new ObservableCollection<T>();

        builder(item => { collection.Add(item); return true; });

        return collection;
    }

    public
#if NET5_0_OR_GREATER
        override
#else
        new
#endif
        ObservableCollection<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return new();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var collection = new ObservableCollection<T>();

        builder(item => { collection.Add(item); return true; }, in state);

        return collection;
    }

#if !NET5_0_OR_GREATER
    protected override Collection<T> NewCollection<T>(List<T>? list = null)
        => list == null ? new ObservableCollection<T>() : new ObservableCollection<T>(list);
#endif
}