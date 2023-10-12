namespace IT.Collections.Factory;

public abstract class EnumerableFactory : IEnumerableFactory
{
    public virtual IEnumerable<T> Empty<T>() => New<T>(0);

    public IEnumerable<T> New<T, TState>(int capacity, in TState state, EnumerableBuilder<T, TState> builder)
    {
        if (capacity == 0) return Empty<T>();

        var enumerable = New<T>(capacity);

        builder(enumerable, in state);

        return enumerable;
    }

    protected abstract IEnumerable<T> New<T>(int capacity);
}