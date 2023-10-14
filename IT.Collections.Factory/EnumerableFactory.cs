#if NET5_0_OR_GREATER

namespace IT.Collections.Factory;

public abstract class EnumerableFactory : IEnumerableFactory
{
    public abstract EnumerableType Type { get; }

    public abstract IEnumerable<T> Empty<T>();

    public abstract IEnumerable<T> New<T>(int capacity);

    public abstract IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder);

    public abstract IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state);
}

#endif