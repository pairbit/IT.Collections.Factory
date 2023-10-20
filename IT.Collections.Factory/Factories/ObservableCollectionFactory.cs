namespace IT.Collections.Factory.Factories;

public class ObservableCollectionFactory : CollectionFactory
{
    public override Type EnumerableType => typeof(ObservableCollection<>);

    public override EnumerableKind Kind => EnumerableKind.None;

    public
#if NET5_0_OR_GREATER
        override
#else
        new
#endif
        ObservableCollection<T> Empty<T>(in Comparers<T> comparers = default) =>
#if NET5_0_OR_GREATER
        new();
#else
        NewObservableCollection(in comparers);
#endif

    public
#if NET5_0_OR_GREATER
        override
#else
        new
#endif
        ObservableCollection<T> New<T>(int capacity, in Comparers<T> comparers = default) =>
#if NET5_0_OR_GREATER
        new();
#else
        NewObservableCollection(in comparers);
#endif

    public
#if NET5_0_OR_GREATER
        override
#else
        new
#endif
        ObservableCollection<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return
#if NET5_0_OR_GREATER
        new();
#else
        NewObservableCollection(in comparers);
#endif
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var collection =
#if NET5_0_OR_GREATER
        new ObservableCollection<T>();
#else
        NewObservableCollection(in comparers);
#endif

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
        if (capacity == 0) return
#if NET5_0_OR_GREATER
        new();
#else
        NewObservableCollection(in comparers);
#endif
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var collection =
#if NET5_0_OR_GREATER
        new ObservableCollection<T>();
#else
        NewObservableCollection(in comparers);
#endif

        builder(item => { collection.Add(item); return true; }, in state);

        return collection;
    }

#if !NET5_0_OR_GREATER
    protected virtual ObservableCollection<T> NewObservableCollection<T>(in Comparers<T> comparers, IList<T>? list = null)
        => list == null ? new() : new(list);

    protected sealed override Collection<T> NewCollection<T>(IList<T> list, in Comparers<T> comparers)
        => NewObservableCollection(in comparers, list);
#endif
}