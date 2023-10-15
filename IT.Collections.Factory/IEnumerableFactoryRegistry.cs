namespace IT.Collections.Factory;

public interface IEnumerableFactoryRegistry : IReadOnlyEnumerableFactoryRegistry
{
    bool TryRegister(Type type, object factory, RegistrationBehavior behavior);

    void Clear();
}