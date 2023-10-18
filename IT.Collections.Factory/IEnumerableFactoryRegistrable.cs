namespace IT.Collections.Factory;

public interface IEnumerableFactoryRegistrable
{
    Type EnumerableType { get; }

    EnumerableKind Kind { get; }
}