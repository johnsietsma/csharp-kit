using System;

/// <summary>
/// Decorates and ActionCaller. Use this a base class for modify message calls when they are fired.
/// </summary>
public class MessageDecorator : IMessageSender
{
    public IMessageSender MessageSender { get; private set; }

    public Action<object> Action { get { return MessageSender.Action; } }
    public Type MessageType { get { return MessageSender.MessageType; } }
    public Type ParamType { get { return MessageSender.ParamType; } }

    public MessageDecorator( IMessageSender messageSender )
    {
        MessageSender = messageSender;
    }

    public virtual void Send<TMessage,TParam>( TParam param )
    {
        MessageSender.Send<TMessage,TParam>( param );
    }

}
