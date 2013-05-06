using System;
using System.Collections.Generic;
    
/// <summary>
/// An instantiable helper class that can subscribe and unsubscribe from
/// <c>MessageSystem</c> events.
/// </summary>
public class MessageRegister
{
    IList<Action> removals = new List<Action>();

    public void Add( Type messageType, params Action<object>[] actions )
    {
        MessageSystem.Add( messageType, actions );
        removals.Add( () => {
            var localActions = actions;
            MessageSystem.Remove( messageType, localActions );
        } );
    }

    public void Remove( Type messageType, params Action<object>[] actions )
    {
        MessageSystem.Remove( messageType, actions );
    }

    public void Add<TMessage, TParam>( params Action<TParam>[] actions ) where TMessage : IMessage<TParam> {
        MessageSystem.Add<TMessage, TParam>( actions );
        removals.Add( () => {
            var localActions = actions;
            MessageSystem.Remove<TMessage, TParam>( localActions );
        } );
    }

    public void Remove<TMessage, TParam>( params Action<TParam>[] actions ) where TMessage : IMessage<TParam> {
        MessageSystem.Remove<TMessage, TParam>( actions );
    }

    public void Clear()
    {
        foreach( Action removal in removals ) {
            removal();
        }
        removals.Clear();
    }
}
