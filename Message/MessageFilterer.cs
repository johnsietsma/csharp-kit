using System;


/// <summary>
/// A message decorator to filter messages fromt he MessageSystem.
/// </summary>
public class MessageFilterer : MessageDecorator
{
    // Reuturn true to filter the message
    public delegate bool MessageFilter( Type messageType, Type paramType, object param );

    public MessageFilter Filter { get; private set; }

    public MessageFilterer( IMessageSender messageSender, MessageFilter filter ) : base( messageSender )
    {
        Filter = filter;
    }

    public override void Send<TMessage,TParam>( TParam param )
    {
        // Apply the filter
        if( !Filter( MessageType, ParamType, param ) ) {
            base.Send<TMessage,TParam>( param );
        }
    }
}