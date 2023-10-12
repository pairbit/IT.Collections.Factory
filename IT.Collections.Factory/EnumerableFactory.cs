namespace IT.Collections.Factory;

public abstract class EnumerableFactory : IEnumerableFactory
{
    public bool IsReadOnly => false;

    public virtual IEnumerable<T> Empty<T>() => New<T>(0);

    public abstract IEnumerable<T> New<T>(int capacity);

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return Empty<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var enumerable = New<T>(capacity);

        builder(enumerable);

        return enumerable;
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return Empty<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var enumerable = New<T>(capacity);

        builder(enumerable, in state);

        return enumerable;
    }
}