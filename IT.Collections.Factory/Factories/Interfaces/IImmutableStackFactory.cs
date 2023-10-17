#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public interface IImmutableStackFactory : IEnumerableFactory
{
    new IImmutableStack<T> Empty<T>(in Comparers<T> comparers = default);

    new IImmutableStack<T> New<T>(int capacity, in Comparers<T> comparers = default);

    new IImmutableStack<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default);

    new IImmutableStack<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default);
}

#endif