namespace IT.Collections.Factory.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        //EnumerableFactoryRegistry.RegisterEnumerableFactory(LinkedListFactory.Default, typeof(IReadOnlyCollection<>));
        //EnumerableFactoryRegistry.RegisterEnumerableFactory(StackFactory.Default, typeof(IEnumerable<>), typeof(IReadOnlyCollection<>));
        EnumerableFactoryRegistry.RegisterEnumerableFactory(EquatableListFactory.Default);

        var enumerableFactory = EnumerableFactoryRegistry.GetEnumerableFactory(typeof(IList<>));

        var factory2 = EnumerableFactoryRegistry.TryGetEnumerableFactory<IList<int>, int>();

        //factory2.
        //var list = (IList<int>)enumerableFactory.New<int>(10);

        Assert.Pass();
    }
}