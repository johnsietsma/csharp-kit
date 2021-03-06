using System;
using System.Collections.Generic;
using CircularBuffer;


public struct MessageReceipt
{
    public Type MessageType;
    public Type ParamType;

    public override string ToString() {
        return string.Format("[MessageReceipt: {0}({1})]", MessageType, ParamType );
    }
}

[Serializable]
public class MessageLog : CircularBuffer<MessageReceipt>
{
    public MessageLog( int capacity ) : base(capacity,true) {}
}

public class MessageLogger : MessageDecorator
{
    private MessageLog log;

    public IEnumerable<MessageReceipt> MessageReceipts {
        get { return log; }
    }

    public MessageLogger( IMessageSender messageSender, int capacity ) : this( messageSender, new MessageLog(capacity) )
    {
    }

    public MessageLogger( IMessageSender messageSender, MessageLog log ) : base( messageSender )
    {
        this.log = log;
    }

    public override void Send<TMessage,TParam>( TParam param )
    {
        log.Put( new MessageReceipt() {
            MessageType = typeof(TMessage),
            ParamType = typeof(TParam)
            } );
        base.Send<TMessage,TParam>( param );
    }
}
