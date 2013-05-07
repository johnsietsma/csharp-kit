using System;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class TestMessageLogger
{
    [Test]
    public void TestFilter_True()
    {
        MessageSystem.Clear();

        IMessageSender sender = MessageSenderBuilder<Message1>.Create<object>( (o)=>{} );
        MessageLogger logger = new MessageLogger( sender, 2 );

        MessageSystem.Add( logger );

        MessageSystem.Send<Message1,object>( null );
        Assert.AreEqual( 1, logger.MessageReceipts.Count() );
        MessageSystem.Send<Message1,object>( null );
        Assert.AreEqual( 2, logger.MessageReceipts.Count() );
        MessageSystem.Send<Message1,object>( null );
        Assert.AreEqual( 2, logger.MessageReceipts.Count() );
    }

    private class Message1 : IMessage<object> {}
}