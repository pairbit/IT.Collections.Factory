namespace IT.Collections.Factory.Factories;

public class ArrayFactory : IEnumerableFactory
{
    public static readonly ArrayFactory Default = new();

    public bool IsReadOnly => false;

    public IEnumerable<T> Empty<T>() => Array.Empty<T>();

    public IEnumerable<T> New<T>(int capacity) => new T[capacity];

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return Array.Empty<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var array = new T[capacity];
        var index = 0;

        builder(item => array[index++] = item, false);

        return array;
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return Array.Empty<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var array = new T[capacity];
        var index = 0;

        builder(item => array[index++] = item, false, in state);

        return array;
    }
}