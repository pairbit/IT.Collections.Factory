#if !NET461

namespace IT.Collections.Factory.Generic;

internal interface IEnumerableTupleKeyValueFactory<TEnumerable, TKey, TValue> 
    : IEnumerableFactory<TEnumerable, (TKey, TValue)>
    where TEnumerable : IEnumerable<(TKey, TValue)>
{

}

#endif