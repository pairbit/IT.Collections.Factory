namespace IT.Collections.Factory.Factories;

public class ArrayFactory :
#if NET5_0_OR_GREATER
    EnumerableFactory
#else
    IEnumerableFactory
#endif
{
    public static readonly ArrayFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.None;

    public
#if NET5_0_OR_GREATER
        override
#endif
        T[] Empty<T>() => Array.Empty<T>();

    public
#if NET5_0_OR_GREATER
        override
#endif
        T[] New<T>(int capacity) => new T[capacity];

    public
#if NET5_0_OR_GREATER
        override
#endif
        T[] New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return Array.Empty<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var array = new T[capacity];
        var index = 0;

        builder(item => { array[index++] = item; return true; });

        return array;
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        T[] New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return Array.Empty<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var array = new T[capacity];
        var index = 0;

        builder(item => { array[index++] = item; return true; }, in state);

        return array;
    }
#if !NET5_0_OR_GREATER
    IEnumerable<T> IEnumerableFactory.Empty<T>() => Empty<T>();
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity) => New<T>(capacity);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder) => New(capacity, builder);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state) => New(capacity, builder, in state);
#endif
}