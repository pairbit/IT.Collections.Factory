﻿namespace IT.Collections.Factory.Factories;

public class ArrayFactory : EnumerableFactory
{
    public static readonly ArrayFactory Default = new();

    public override IEnumerable<T> Empty<T>() => Array.Empty<T>();

    public override IEnumerable<T> New<T>(int capacity) => new T[capacity];
}