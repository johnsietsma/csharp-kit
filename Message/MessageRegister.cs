using System;
using System.Collections.Generic;

/// <summary>
/// An instantiable helper class that can subscribe and unsubscribe from
/// <c>MessageSystem</c> events.
/// </summary>
public class MessageRegister
{
    List<IMessageSender> removals = new List<IMessageSender>();

    public void Add<TParam>( Type messageType, params Action<TParam>[] actions )
    {
        IMessageSender[] senders = MessageSystem.Add<TParam>( messageType, actions );
        senders.ForEach( s=>removals.Add( s ) );
    }

    public void Add<TMessage, TParam>( params Action<TParam>[] actions ) where TMessage : IMessage<TParam> {
        Add<TParam>( typeof( TMessage ), actions );
    }

    public void Clear()
    {
        MessageSystem.Remove( removals.ToArray() );
        removals.Clear();
    }
}
