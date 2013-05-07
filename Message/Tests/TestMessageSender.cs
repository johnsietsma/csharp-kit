using NUnit.Framework;
using System;

[TestFixture]
public class TestMessageSender
{
    [Test]
    public void TestGetAction()
    {
        Action<string> stringDel = s=>{};
        IMessageSender sender = MessageSender.Create<Message1,string>( stringDel );
        Assert.AreEqual( stringDel, sender.GetAction<string>() );
    }

    [Test]
    public void TestGetAction_AssignableTypes()
    {
        Action<string> stringDel = o=>{};
        IMessageSender sender = MessageSender.Create<Message1,string>( stringDel );
        Assert.Throws<CheckException>( ()=>sender.GetAction<object>() );
    }    

    public class Message1 : IMessage<string> {}
}