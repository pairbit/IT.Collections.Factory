#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public class ImmutableArrayFactory : IEnumerableFactory
{
    public static readonly ImmutableArrayFactory Default = new();

    public EnumerableType Type => EnumerableType.None;

    public IEnumerable<T> Empty<T>() => ImmutableArray<T>.Empty;

    public IEnumerable<T> New<T>(int capacity)
    {
        if (capacity == 0) return ImmutableArray<T>.Empty;

        return ImmutableArray.Create(new T[capacity]);
    }

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return ImmutableArray<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var array = new T[capacity];
        var index = 0;

        builder(item => { array[index++] = item; return true; }, false);

        return ImmutableArray.Create(array);
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return ImmutableArray<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var array = new T[capacity];
        var index = 0;

        builder(item => { array[index++] = item; return true; }, false, in state);

        return ImmutableArray.Create(array);
    }
}

#endif