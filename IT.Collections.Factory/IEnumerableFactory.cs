namespace IT.Collections.Factory;

public interface IEnumerableFactory : IEnumerableFactoryRegistrable
{
    IEnumerable<T> Empty<T>(in Comparers<T> comparers = default);

    IEnumerable<T> New<T>(int capacity, in Comparers<T> comparers = default);

    IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default);

    IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default);
}