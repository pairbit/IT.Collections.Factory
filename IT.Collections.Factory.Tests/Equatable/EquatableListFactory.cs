﻿using IT.Collections.Equatable;

namespace IT.Collections.Factory.Tests;

public class EquatableListFactory : EnumerableFactory
{
    public static readonly EquatableListFactory Default = new();

    public override IEnumerable<T> New<T>(int capacity) => new EquatableList<T>(capacity);
}