#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public class ImmutableArrayFactory : IEnumerableFactory
{
    public static readonly ImmutableArrayFactory Default = new();

    public EnumerableType Type => EnumerableType.Fixed;

    public ImmutableArray<T> Empty<T>(in Comparers<T> comparers = default) => ImmutableArray<T>.Empty;

    public ImmutableArray<T> New<T>(int capacity, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ImmutableArray<T>.Empty;

        return ImmutableArray.Create(new T[capacity]);
    }

    public ImmutableArray<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ImmutableArray<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var array = new T[capacity];
        var index = 0;

        builder(item => { array[index++] = item; return true; });

        return ImmutableArray.Create(array);
    }

    public ImmutableArray<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ImmutableArray<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var array = new T[capacity];
        var index = 0;

        builder(item => { array[index++] = item; return true; }, in state);

        return ImmutableArray.Create(array);
    }

    IEnumerable<T> IEnumerableFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
}

#endif