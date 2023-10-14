using System.Collections.ObjectModel;

namespace IT.Collections.Factory.Factories;

public class ObservableCollectionFactory :
#if NET5_0_OR_GREATER
    EnumerableFactory
#else
    IEnumerableFactory
#endif
{
    public static readonly ObservableCollectionFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.None;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ObservableCollection<T> Empty<T>() => new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        ObservableCollection<T> New<T>(int capacity)
    {
        if (capacity == 0) return new();

        return new(new List<T>(capacity));
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        ObservableCollection<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return new();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list = new List<T>(capacity);

        builder(item => { list.Add(item); return true; });

        return new(list);
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        ObservableCollection<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return new();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list = new List<T>(capacity);

        builder(item => { list.Add(item); return true; }, in state);

        return new(list);
    }
#if !NET5_0_OR_GREATER
    IEnumerable<T> IEnumerableFactory.Empty<T>() => Empty<T>();
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity) => New<T>(capacity);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder) => New(capacity, builder);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state) => New(capacity, builder, in state);
#endif
}