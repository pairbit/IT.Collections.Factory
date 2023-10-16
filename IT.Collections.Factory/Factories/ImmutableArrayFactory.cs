#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public class ImmutableArrayFactory : IEnumerableFactory
{
    public static readonly ImmutableArrayFactory Default = new();

    public EnumerableType Type => EnumerableType.Fixed;

    public ImmutableArray<T> Empty<T>() => ImmutableArray<T>.Empty;

    public ImmutableArray<T> New<T>(int capacity)
    {
        if (capacity == 0) return ImmutableArray<T>.Empty;

        return ImmutableArray.Create(new T[capacity]);
    }

    public ImmutableArray<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return ImmutableArray<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var array = new T[capacity];
        var index = 0;

        builder(item => { array[index++] = item; return true; });

        return ImmutableArray.Create(array);
    }

    public ImmutableArray<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return ImmutableArray<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var array = new T[capacity];
        var index = 0;

        builder(item => { array[index++] = item; return true; }, in state);

        return ImmutableArray.Create(array);
    }

    IEnumerable<T> IEnumerableFactory.Empty<T>() => Empty<T>();
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity) => New<T>(capacity);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder) => New(capacity, builder);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state) => New(capacity, builder, in state);
}

#endif