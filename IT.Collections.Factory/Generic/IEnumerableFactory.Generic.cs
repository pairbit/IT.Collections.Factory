namespace IT.Collections.Factory.Generic;

public delegate TEnumerable EnumerableFactory<TEnumerable, T>(int capacity) where TEnumerable : IEnumerable<T>;

public interface IEnumerableFactory<TEnumerable, T> where TEnumerable : IEnumerable<T>
{
    TEnumerable Empty();

    TEnumerable New<TState>(int capacity, in TState state, EnumerableBuilder<T, TState> builder);
}