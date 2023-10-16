#if NET5_0_OR_GREATER

namespace IT.Collections.Factory;

public abstract class EnumerableFactory : IEnumerableFactory
{
    public abstract EnumerableType Type { get; }

    public abstract IEnumerable<T> Empty<T>(in Comparers<T> comparers = default);

    public abstract IEnumerable<T> New<T>(int capacity, in Comparers<T> comparers = default);

    public abstract IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default);

    public abstract IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default);
}

#endif