namespace IT.Collections.Factory.Generic;

public delegate TEnumerable EnumerableFactory<TEnumerable, T>(int capacity) where TEnumerable : IEnumerable<T>;

public interface IEnumerableFactory<TEnumerable, T> : IEnumerableFactoryRegistrable
    where TEnumerable : IEnumerable<T>
{
    TEnumerable Empty();

    TEnumerable New(int capacity);

    TEnumerable New(int capacity, EnumerableBuilder<T> builder);

    TEnumerable New<TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state);
}