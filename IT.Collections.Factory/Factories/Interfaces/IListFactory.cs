namespace IT.Collections.Factory.Factories;

public interface IListFactory : ICollectionFactory
{
    new IList<T> Empty<T>(in Comparers<T> comparers = default);

    new IList<T> New<T>(int capacity, in Comparers<T> comparers = default);

    new IList<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default);

    new IList<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default);
}