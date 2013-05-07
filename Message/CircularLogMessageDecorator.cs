using System;
using System.Collections.Generic;
using CircularBuffer;


public struct MessageReceipt 
{
    public Type senderType;
}

public class CircularLogMessageDecorator : MessageDecorator
{
    private CircularBuffer<MessageReceipt> messageReceipts;

    public IEnumerable<MessageReceipt> MessageReceipts {
        get { return messageReceipts; }
    }

    public CircularLogMessageDecorator( IMessageSender messageSender, int capacity ) : base( messageSender )
    {
        messageReceipts = new CircularBuffer<MessageReceipt>( capacity, true );
    }

    public override void Send<TMessage,TParam>( TParam param )
    {
        messageReceipts.Put( new MessageReceipt() );
        base.Send<TMessage,TParam>( param );
    }
}