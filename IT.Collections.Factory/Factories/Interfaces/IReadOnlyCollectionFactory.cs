namespace IT.Collections.Factory.Factories;

public interface IReadOnlyCollectionFactory : IEnumerableFactory
{
    new IReadOnlyCollection<T> Empty<T>(in Comparers<T> comparers = default);

    new IReadOnlyCollection<T> New<T>(int capacity, in Comparers<T> comparers = default);

    new IReadOnlyCollection<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default);

    new IReadOnlyCollection<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default);
}