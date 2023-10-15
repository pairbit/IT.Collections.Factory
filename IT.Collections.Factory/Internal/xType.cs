using IT.Collections.Factory;
using System.Runtime.CompilerServices;

namespace System;

internal static class xType
{
    internal static readonly Type IEnumerableFactoryType = typeof(IEnumerableFactory);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsAssignableFromEnumerableFactory(this Type type) => type.IsAssignableFrom(IEnumerableFactoryType);
}