using System;

/// <summary>
/// Decorates and ActionCaller. Use this a base class for modify message calls when they are fired.
/// </summary>
public class MessageDecorator : IMessageSender
{
    public IMessageSender MessageSender { get; private set; }

    public Type MessageType { get { return MessageSender.MessageType; } }
    public Type ParamType { get { return MessageSender.ParamType; } }
    public object OriginalAction { get { return MessageSender.OriginalAction; } }
    public Action<T> GetAction<T>() { return MessageSender.GetAction<T>(); }

    public MessageDecorator( IMessageSender messageSender )
    {
        MessageSender = messageSender;
    }

    public virtual void Send<TMessage,TParam>( TParam param )
    {
        MessageSender.Send<TMessage,TParam>( param );
    }

}
