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

        var list = (IList<int>)enumerableFactory.Empty<int>();



        Assert.Pass();
    }
}