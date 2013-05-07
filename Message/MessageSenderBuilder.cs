using System;

public static class MessageSenderBuilder<TMessage>
{
    public static IMessageSender Create<TParam>( Action<TParam> sender )
    {
        return Create<TParam>( typeof(TMessage), sender );
    }

    public static IMessageSender Create<TParam>( Type messageType, Action<TParam> sender )
    {
        Check.NotNull( sender );
        return MessageSender.Create<TParam>( messageType, sender );
    }
}