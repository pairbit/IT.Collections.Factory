using System.Collections.Concurrent;

namespace IT.Collections.Factory.Factories;

public class ConcurrentStackFactory : EnumerableFactory
{
    public static readonly ConcurrentStackFactory Default = new();

    public override IEnumerable<T> New<T>(int capacity) => new ConcurrentStack<T>();
}