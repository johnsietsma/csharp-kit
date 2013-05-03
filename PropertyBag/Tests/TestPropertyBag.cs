using NUnit.Framework;

[TestFixture]
public class TestPropertyBag
{
    [Test]
    public void TestGetValue()
    {
        PropertyBag pb = new PropertyBag();
        pb["Test"].Value = "test";
        Assert.AreEqual( pb["Test"].Value, "test" );
    }

    [Test]
    public void TestToString()
    {
        PropertyBag pb = new PropertyBag();
        pb["Test"].Value = "test";

        Assert.AreEqual(  pb.ToString(), "[PropertyBag: [Property Test:test]]" );
    }
}