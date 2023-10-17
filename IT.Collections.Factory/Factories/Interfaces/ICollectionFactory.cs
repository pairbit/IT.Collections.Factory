namespace IT.Collections.Factory.Factories;

public interface ICollectionFactory : IEnumerableFactory
{
    new ICollection<T> Empty<T>(in Comparers<T> comparers = default);

    new ICollection<T> New<T>(int capacity, in Comparers<T> comparers = default);

    new ICollection<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default);

    new ICollection<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default);
}