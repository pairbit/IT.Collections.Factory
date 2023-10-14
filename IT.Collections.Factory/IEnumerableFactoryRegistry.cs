namespace IT.Collections.Factory;

public interface IEnumerableFactoryRegistry : IReadOnlyEnumerableFactoryRegistry
{
    bool Register(Type type, object factory, bool overwrite);

    void Clear();
}