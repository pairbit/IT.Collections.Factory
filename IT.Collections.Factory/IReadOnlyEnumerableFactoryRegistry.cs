namespace IT.Collections.Factory;

public interface IReadOnlyEnumerableFactoryRegistry : IReadOnlyDictionary<Type, IEnumerableFactoryRegistrable>
{
    TFactory? TryGetFactory<TFactory>() where TFactory : IEnumerableFactoryRegistrable;
}