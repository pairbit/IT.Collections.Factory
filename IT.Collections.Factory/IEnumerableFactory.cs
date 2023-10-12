namespace IT.Collections.Factory;

public interface IEnumerableFactory
{
    IEnumerable<T> Empty<T>();

    IEnumerable<T> New<T, TState>(int capacity, in TState state, EnumerableBuilder<T, TState> builder);

    //IEnumerable<T> New<T>(int capacity);
}