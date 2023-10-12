﻿namespace IT.Collections.Factory.Factories;

public class ListFactory : EnumerableFactory
{
    public static readonly ListFactory Default = new();

    public override IEnumerable<T> Empty<T>() => new List<T>();

    public override IEnumerable<T> New<T>(int capacity) => new List<T>(capacity);
}