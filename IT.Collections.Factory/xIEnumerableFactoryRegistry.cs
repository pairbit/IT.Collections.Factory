namespace IT.Collections.Factory;

using Internal;

public static class xIEnumerableFactoryRegistry
{
    //public static void Register(this IEnumerableFactoryRegistry registry, IEnumerableFactory factory, Type factoryType)
    //{
    //    if (registry == null) throw new ArgumentNullException(nameof(registry));
    //    if (factory == null) throw new ArgumentNullException(nameof(factory));
    //    if (!factoryType.IsAssignableFromEnumerableFactory()) throw Ex.NotAssignableFromEnumerableFactory(factoryType, nameof(factoryType));

    //    var enumerable = factory.Empty<int>();

    //    if (enumerable == null) throw new ArgumentException("Factory Error", nameof(factory));

    //    var enumerableType = enumerable.GetType();

    //    Type baseType;

    //    if (enumerableType.IsArray)
    //    {
    //        baseType = typeof(Array);
    //    }
    //    else
    //    {
    //        if (!enumerableType.IsGenericType) throw new ArgumentException($"Registered type '{enumerableType.FullName}' is not generic type", nameof(factory));

    //        baseType = enumerableType.GetGenericTypeDefinition();
    //    }

    //    _enumerableFactories[baseType] = factory;

    //    if (genericTypes != null && genericTypes.Length > 0)
    //    {
    //        for (int i = 0; i < genericTypes.Length; i++)
    //        {
    //            var genericType = genericTypes[i];

    //            if (!genericType.IsGenericType)
    //                throw new ArgumentException($"Registered type '{genericType.FullName}' is not generic type", nameof(genericTypes));

    //            var type = genericType.MakeGenericType(typeof(int));

    //            if (!typeof(IEnumerable<int>).IsAssignableFrom(type))
    //                throw new ArgumentException($"Registered type '{genericType.FullName}' does not inherit type '{typeof(IEnumerable<>).FullName}'", nameof(genericTypes));

    //            if (!type.IsAssignableFrom(enumerableType))
    //                throw new ArgumentException($"Registered type '{genericType.FullName}' is not assignable from type '{baseType.FullName}'", nameof(genericTypes));

    //            _enumerableFactories[genericType] = factory;
    //        }
    //    }
    //}

    //public static void Register(this IEnumerableFactoryRegistry registry, IEnumerableFactory factory)
    //{
    //    if (registry == null) throw new ArgumentNullException(nameof(registry));
    //    if (factory == null) throw new ArgumentNullException(nameof(factory));

    //    registry.Register(factory.GetType(), factory);
    //}
}