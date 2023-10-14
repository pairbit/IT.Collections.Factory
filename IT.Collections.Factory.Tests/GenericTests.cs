using IT.Collections.Equatable;
using IT.Collections.Factory.Generic;

namespace IT.Collections.Factory.Tests;

public class GenericTests
{
    private int _capacity;

    [Test]
    public void Test()
    {
        EnumerableFactoryRegistry.RegisterEnumerableFactory<List<string>, string>(
            x => new EquatableList<string>(x, StringComparer.OrdinalIgnoreCase), (list, item) => list.Add(item));

        var factory = EnumerableFactoryRegistry.GetEnumerableFactory<List<string>, string>();

        FactoryTest(factory);
    }

    private void FactoryTest<TEnumerable, T>(IEnumerableFactory<TEnumerable, T> factory) where TEnumerable : IEnumerable<T>
    {
        var empty = factory.Empty();

        Assert.That(empty.Any(), Is.False);

        var type = empty.GetType();

        if (factory.Type != EnumerableType.None)
            Console.WriteLine($"Type '{type.GetGenericTypeDefinition().FullName}' is {factory.Type}");

        if (factory.Type.IsReadOnly())
        {
            Assert.Throws<NotSupportedException>(() => factory.New(0));
            Assert.Throws<NotSupportedException>(() => factory.New(_capacity));
        }
        else
        {
            Assert.That(factory.New(0).Any(), Is.False);

            var withCapacity = factory.New(_capacity);

            //Assert.That(withCapacity.Any(), Is.False);
            Assert.That(withCapacity.GetType(), Is.EqualTo(type));
        }

        Assert.That(factory.New(0, null!).Any(), Is.False);
        Assert.That(factory.New(0, null!, 0).Any(), Is.False);
        Assert.Throws<ArgumentNullException>(() => factory.New(1, null!));
        Assert.Throws<ArgumentNullException>(() => factory.New(1, null!, 0));
    }
}