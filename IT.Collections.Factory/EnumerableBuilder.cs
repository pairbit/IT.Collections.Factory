namespace IT.Collections.Factory;

public delegate void EnumerableBuilder<T>(IEnumerable<T> buffer);
public delegate void EnumerableBuilder<T, TState>(IEnumerable<T> buffer, in TState state);