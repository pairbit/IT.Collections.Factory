namespace IT.Collections.Factory.Tests.Internal;

internal static class xException
{
    public static void CheckCapacity(this ArgumentOutOfRangeException ex)
    {
        Assert.That(ex.ParamName, Is.EqualTo("capacity"));
        //Assert.That(ex.ActualValue, Is.EqualTo(-99));
    }

    public static void CheckBuilder(this ArgumentException ex)
    {
        Assert.That(ex.ParamName, Is.EqualTo("builder"));
    }
}