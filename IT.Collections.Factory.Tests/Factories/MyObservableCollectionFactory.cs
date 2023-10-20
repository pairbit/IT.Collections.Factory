#if !NET5_0_OR_GREATER

using IT.Collections.Factory.Factories;
using System.Collections.ObjectModel;

namespace IT.Collections.Factory.Tests;

public class MyObservableCollection<T> : ObservableCollection<T>
{
    public MyObservableCollection() { }
    public MyObservableCollection(IList<T> list) : base(list) { }
}

public class MyObservableCollectionFactory : ObservableCollectionFactory
{
    public override Type EnumerableType => typeof(MyObservableCollection<>);

    protected override ObservableCollection<T> NewObservableCollection<T>(in Comparers<T> comparers, IList<T>? list = null)
        => list == null ? new MyObservableCollection<T>() : new MyObservableCollection<T>(list);
}

#endif