﻿using System.Collections.Concurrent;

namespace IT.Collections.Factory.Factories;

public class ConcurrentBagFactory : EnumerableFactory
{
    public static readonly ConcurrentBagFactory Default = new();

    public override IEnumerable<T> New<T>(int capacity) => new ConcurrentBag<T>();
}