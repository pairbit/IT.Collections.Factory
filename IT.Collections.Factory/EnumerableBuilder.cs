namespace IT.Collections.Factory;

public delegate void EnumerableBuilder<T, TState>(IEnumerable<T> buffer, in TState state);