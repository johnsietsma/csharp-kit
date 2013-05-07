using System;
using NUnit.Framework;

[TestFixture]
public class TestChecks
{
    [Test]
    public void TestFalse()
    {
        Assert.Throws( typeof( CheckException ), ()=>Check.False( true ) );
    }

    [Test]
    public void TestEqual_int()
    {
        Assert.Throws< CheckException >( ()=>Check.Equal( 1,2 ) );
        Assert.DoesNotThrow( ()=>Check.Equal( 1,1 ) );
    }

    [Test]
    public void TestEqual_object()
    {
        object o1 = new object();
        object o2 = new object();
        Assert.Throws<CheckException>( ()=>Check.Equal( o1, o2 ) );
        Assert.DoesNotThrow( ()=>Check.Equal( o1, o1 ) );
    }
}
