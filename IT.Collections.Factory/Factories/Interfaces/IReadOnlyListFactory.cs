namespace IT.Collections.Factory.Factories;

public interface IReadOnlyListFactory : IReadOnlyCollectionFactory
{
    new IReadOnlyList<T> Empty<T>(in Comparers<T> comparers = default);

    new IReadOnlyList<T> New<T>(int capacity, in Comparers<T> comparers = default);

    new IReadOnlyList<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default);

    new IReadOnlyList<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default);
}