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
        EnumerableFactoryRegistry.RegisterEnumerable(EquatableListFactory.Default);

        //var enumerableFactory = EnumerableFactoryRegistry.GetFactory(typeof(IList<>));



        Assert.Pass();
    }
}