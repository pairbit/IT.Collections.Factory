#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public class ImmutableListFactory : IEnumerableFactory
{
    public static readonly ImmutableListFactory Default = new();

    public bool IsReadOnly => false;

    public IEnumerable<T> Empty<T>() => ImmutableList<T>.Empty;

    public IEnumerable<T> New<T>(int capacity) => ImmutableList<T>.Empty;

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return ImmutableList<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var listBuilder = ImmutableList<T>.Empty.ToBuilder();

        builder(listBuilder.Add, false);

        return listBuilder.ToImmutable();
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return ImmutableList<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var listBuilder = ImmutableList<T>.Empty.ToBuilder();

        builder(listBuilder.Add, false, in state);

        return listBuilder.ToImmutable();
    }
}

#endif