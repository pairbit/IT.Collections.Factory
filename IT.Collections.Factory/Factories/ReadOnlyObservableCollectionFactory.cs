using System.Collections.ObjectModel;

namespace IT.Collections.Factory.Factories;

public class ReadOnlyObservableCollectionFactory :
#if NET5_0_OR_GREATER
    EnumerableFactory
#else
    IEnumerableFactory
#endif
{
    public static readonly ReadOnlyObservableCollectionFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.ReadOnly;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ReadOnlyObservableCollection<T> Empty<T>(in Comparers<T> comparers = default) => Cache<T>.Empty;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ReadOnlyObservableCollection<T> New<T>(int capacity, in Comparers<T> comparers = default)
    {
        throw new NotSupportedException();
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        ReadOnlyObservableCollection<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return Cache<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var collection = new ObservableCollection<T>();

        builder(item => { collection.Add(item); return true; });

        return new(collection);
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        ReadOnlyObservableCollection<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return Cache<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var collection = new ObservableCollection<T>();

        builder(item => { collection.Add(item); return true; }, in state);

        return new(collection);
    }

    static class Cache<T>
    {
        public readonly static ReadOnlyObservableCollection<T> Empty = new(new ObservableCollection<T>());
    }
#if !NET5_0_OR_GREATER
    IEnumerable<T> IEnumerableFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
#endif
}