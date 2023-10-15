namespace IT.Collections.Factory;

public interface IEnumerableFactory : IEnumerableFactoryRegistrable
{
    IEnumerable<T> Empty<T>();

    IEnumerable<T> New<T>(int capacity);

    IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder);

    IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state);
}