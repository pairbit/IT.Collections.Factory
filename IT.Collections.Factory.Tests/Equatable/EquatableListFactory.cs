using IT.Collections.Equatable;

namespace IT.Collections.Factory.Tests;

public class EquatableListFactory : IEnumerableFactory
{
    public static readonly EquatableListFactory Default = new();

    public EnumerableType Type => EnumerableType.None;

    public IEnumerable<T> Empty<T>() => new EquatableList<T>();

    public IEnumerable<T> New<T>(int capacity) => new EquatableList<T>(capacity);

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return new EquatableList<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list = new EquatableList<T>(capacity);

        builder(item => { list.Add(item); return true; });

        return list;
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return new EquatableList<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list = new EquatableList<T>(capacity);

        builder(item => { list.Add(item); return true; }, in state);

        return list;
    }
}