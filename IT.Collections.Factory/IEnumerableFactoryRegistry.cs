namespace IT.Collections.Factory;

public interface IEnumerableFactoryRegistry : IReadOnlyEnumerableFactoryRegistry
{
    void Clear();

    bool TryRegisterFactory<TFactory>(TFactory factory, RegistrationBehavior behavior) where TFactory : IEnumerableFactoryRegistrable;
}