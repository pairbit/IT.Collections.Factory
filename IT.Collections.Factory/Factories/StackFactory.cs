namespace IT.Collections.Factory.Factories;

public class StackFactory : IEnumerableFactory
{
    public static readonly StackFactory Default = new();

    public EnumerableType Type => EnumerableType.Reverse;

    public IEnumerable<T> Empty<T>() => new Stack<T>();

    public IEnumerable<T> New<T>(int capacity) => new Stack<T>(capacity);

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return new Stack<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var stack = new Stack<T>(capacity);

        builder(item => { stack.Push(item); return true; }, true);

        return stack;
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return new Stack<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var stack = new Stack<T>(capacity);

        builder(item => { stack.Push(item); return true; }, true, in state);

        return stack;
    }
}