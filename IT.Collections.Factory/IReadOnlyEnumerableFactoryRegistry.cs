namespace IT.Collections.Factory;

public interface IReadOnlyEnumerableFactoryRegistry : IReadOnlyDictionary<Type, IEnumerableFactoryRegistrable>
{
    TFactory? TryGet<TFactory>() where TFactory : IEnumerableFactoryRegistrable;
}