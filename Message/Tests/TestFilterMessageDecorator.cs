using System;
using NUnit.Framework;

[TestFixture]
public class TestFilterMessageDecorator
{
    [Test]
    public void TestFilter_True()
    {
        bool isCalled = false;
        Action<object> action1 = (o)=>isCalled=true;

        IMessageSender sender = MessageSender.Create<Message1,object>( action1 );
        sender = new FilterMessageDecorator( sender, (mt,pt,p)=>!mt.Name.EndsWith("1") );

        MessageSystem.Add<Message1>( sender );

        MessageSystem.Send<Message1,object>( null );

        Assert.IsFalse( isCalled, "Message shouldn't be called" );
    }

    private class Message1 : IMessage<object> {}
}