namespace IT.Collections.Factory;

public interface IEnumerableFactoryRegistry : IReadOnlyEnumerableFactoryRegistry
{
    void Clear();

    bool TryRegister<TFactory>(TFactory factory, RegistrationBehavior behavior) where TFactory : IEnumerableFactoryRegistrable;
}