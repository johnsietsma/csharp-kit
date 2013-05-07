using System;

/// <summary>
/// An object that sends a message within the MessageSystem.
/// </summary>
public class MessageSender : IMessageSender
{
    public delegate IMessageSender FilterDelegate( IMessageSender sender );

    // The Type of the message that is being sent
    public Type MessageType { get; private set; }

    // The Type of the param passed into the Action
    public Type ParamType { get; private set; }

    public Action<object> Action { get; private set; }

    public object OriginalAction { get; private set; }

    // This actually 'sends' the message.
    public Action<T> GetAction<T>()
    {
        Action<T> action = OriginalAction as Action<T>;
        Check.NotNull( action, "Cannot convert action to Type " + typeof(T).Name );
        return action;
    }

    private MessageSender() {} // not allowed

    public static MessageSender Create<TMessage,TParam>( Action<TParam> action ) 
    {
        return Create<TParam>( typeof(TMessage), action );
    }

    public static MessageSender Create<TParam>( Type messageType, Action<TParam> action )
    {
        Check.NotNull( action, "Please provide a valid action" );
        MessageSender sender = new MessageSender();
        sender.MessageType = messageType;
        sender.ParamType = typeof(TParam);
        sender.Action = (o)=>action((TParam)o);
        sender.OriginalAction = action;
        return sender;
    }

    /// <summary>
    /// Send the message by calling the contained Action.
    /// The TMessage generic param isn't used, but is provided for subclasses that want the meta-info.
    /// </summary>
    public void Send<TMessage,TParam>( TParam param )
    {
        Action( param );
    }
}
