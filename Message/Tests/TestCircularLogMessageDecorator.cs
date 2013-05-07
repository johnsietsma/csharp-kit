using System;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class TestCircularLogMessageDecorator
{
    [Test]
    public void TestFilter_True()
    {
        IMessageSender sender = MessageSender.Create<Message1,object>( ()=>{} );
        CircularLogMessageDecorator logDecorator = new CircularLogMessageDecorator( sender, 2 );

        MessageSystem.Add<Message1>( logDecorator );

        MessageSystem.Send<Message1,object>( null );
        Assert.AreEqual( 1, logDecorator.MessageReceipts.Count() );
        MessageSystem.Send<Message1,object>( null );
        Assert.AreEqual( 2, logDecorator.MessageReceipts.Count() );
        MessageSystem.Send<Message1,object>( null );
        Assert.AreEqual( 2, logDecorator.MessageReceipts.Count() );
    }

    private class Message1 : IMessage<object> {}
}