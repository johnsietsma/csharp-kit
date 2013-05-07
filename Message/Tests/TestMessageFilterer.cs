using System;
using NUnit.Framework;

[TestFixture]
public class TestMessageFilterer
{
    [Test]
    public void TestFilter_True()
    {
        bool isCalled = false;
        Action<object> action1 = (o)=>isCalled=true;

        IMessageSender sender = MessageSenderBuilder<Message1>.Create( action1 );
        sender = new MessageFilterer( sender, (mt,pt,p)=>mt.Name.EndsWith("1") );

        MessageSystem.Add( sender );

        MessageSystem.Send<Message1,object>( null );

        Assert.IsFalse( isCalled, "Message shouldn't be called" );
    }

    private class Message1 : IMessage<object> {}
}