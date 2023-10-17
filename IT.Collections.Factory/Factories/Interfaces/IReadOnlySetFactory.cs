#if NET6_0_OR_GREATER

namespace IT.Collections.Factory.Factories;

public interface IReadOnlySetFactory : IReadOnlyCollectionFactory
{
    new IReadOnlySet<T> Empty<T>(in Comparers<T> comparers = default);

    new IReadOnlySet<T> New<T>(int capacity, in Comparers<T> comparers = default);

    new IReadOnlySet<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default);

    new IReadOnlySet<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default);
}

#endif