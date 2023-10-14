﻿namespace System;

internal static class xType
{
    public static Type GetGenericTypeDefinitionOrArray(this Type type)
        => type.IsArray ? typeof(Array) : type.GetGenericTypeDefinition();
}