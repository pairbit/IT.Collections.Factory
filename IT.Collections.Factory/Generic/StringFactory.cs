#if NET6_0_OR_GREATER

namespace IT.Collections.Factory.Generic;

internal class StringFactory : IEnumerableFactory<string, char>
{
    public Type EnumerableType => typeof(string);

    public EnumerableKind Kind => EnumerableKind.Fixed | EnumerableKind.ReadOnly;

    public string Empty() => string.Empty;

    public string New(int capacity) => throw new NotSupportedException();

    public string New(int capacity, EnumerableBuilder<char> builder)
    {
        if (capacity == 0) return string.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        return string.Create(capacity, builder, CreateString23);
    }

    public string New<TState>(int capacity, EnumerableBuilder<char, TState> builder, in TState state)
    {
        throw new NotImplementedException();
    }

    private static void CreateString23(Span<char> span, EnumerableBuilder<char> builder)
    {
        //var i = 0;
        //TODO: Как заполинить?
        //builder(ch => { span[i++] = ch; return true; });
    }
}

#endif