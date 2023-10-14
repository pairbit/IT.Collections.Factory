namespace IT.Collections.Factory;

public delegate bool TryAdd<T>(T item);
public delegate void EnumerableBuilder<T>(TryAdd<T> tryAdd);
public delegate void EnumerableBuilder<T, TState>(TryAdd<T> tryAdd, in TState state);