namespace IT.Collections.Factory.Factories;

public interface IProducerConsumerCollectionFactory : IEnumerableFactory
{
    new IProducerConsumerCollection<T> Empty<T>(in Comparers<T> comparers = default);

    new IProducerConsumerCollection<T> New<T>(int capacity, in Comparers<T> comparers = default);

    new IProducerConsumerCollection<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default);

    new IProducerConsumerCollection<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default);
}