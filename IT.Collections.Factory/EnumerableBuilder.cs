namespace IT.Collections.Factory;

public delegate void EnumerableBuilder<T>(Action<T> add, bool reverse);
public delegate void EnumerableBuilder<T, TState>(Action<T> add, bool reverse, in TState state);