using NUnit.Framework;

[TestFixture]
public class TestMessage
{

    [Test]
    public void Test_Nicify1()
    {
        string fullName = "ArmelloEngine.Transaction`1+Message+Start[[ArmelloEngine.Game, armello-engine, Version=1.0.4891.22498, Culture=neutral, PublicKeyToken=null]]";
        Assert.AreEqual( "Transaction`1.Start", Message.NicifyName( fullName ) );
    }
}
