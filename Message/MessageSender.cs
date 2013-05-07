using System;

/// <summary>
/// An object that sends a message within the MessageSystem.
/// </summary>
public class MessageSender : IMessageSender
{
    // This actually 'sends' the message.
    public Action<object> Action { get; set; }

    // The Type of the message that is being sent
    public Type MessageType { get; private set; }

    // The Type of the param passed into the Action
    public Type ParamType { get; private set; }

    public MessageSender( Action<object> action, Type messageType, Type paramType )
    {
        Action = action;
        MessageType = messageType;
        ParamType = paramType;
    }

    /// <summary>
    /// Send the message by calling the containsed Action.
    /// The TMessage generic param isn't used, but is provided for subclasses that want the meta-info.
    /// </summary>
    public void Send<TMessage,TParam>( TParam param )
    {
        Action( param );
    }

    public static IMessageSender Create<TMessage,TParam>( Action<TParam> sender )
    {
        return Create<TParam>( typeof(TMessage), sender );
    }

    public static IMessageSender Create<TParam>( Type messageType, Action<TParam> sender )
    {
        Check.NotNull( sender );

        MessageSender messageSender = new MessageSender( sender as Action<object>, messageType, typeof(TParam) );
        return messageSender;
    }
}
