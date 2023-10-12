namespace IT.Collections.Factory.Factories;

public class StackFactory : EnumerableFactory
{
    public static readonly StackFactory Default = new();

    public override IEnumerable<T> Empty<T>() => new Stack<T>();

    public override IEnumerable<T> New<T>(int capacity) => new Stack<T>(capacity);
}