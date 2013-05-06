using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// MessageSystem is a type-safe messagin system.
/// </summary>
public static class MessageSystem
{
    /// <summary>
    /// A wrapper that takes a generic listener<T> and put's a non-generic wrapper around it.
    /// This allows a collections of listener<T>s with different Type params.
    /// </summary>
    private class ActionWrapper
    {
        public Action<object> listener { get; set; }
        public Type ParamType { get; private set; }

        public void Call<T>( T param )
        {
            listener( param );
        }

        public static ActionWrapper Create<T>( Action<T> listener )
        {
            Check.NotNull( listener );

            ActionWrapper wrapper = new ActionWrapper();
            wrapper.listener = listener as Action<object>;
            wrapper.ParamType = typeof( T );
            return wrapper;
        }
    }

    /// A dictionary that maps message types to message listeners.
    private static Dictionary<Type, List<ActionWrapper>> typeListenersMap = new Dictionary<Type, List<ActionWrapper>>();

    public static IEnumerable<Action<TParam>> GetListeners<TMessage, TParam>() where TMessage : IMessage<TParam> {
        Type key =typeof( TMessage );
        if( typeListenersMap.ContainsKey( key ) ) { return typeListenersMap[key].Select( aw=>aw.listener as Action<TParam> ); }
        return Enumerable.Empty<Action<TParam>>();
    }


    /// <summary>
    /// Send a message of type TMessage to all listeners.
    /// <param name="param">The parameter to send with the message</param>
    /// </summary>
    public static void Send<TMessage,TParam>( TParam param ) where TMessage : IMessage<TParam> {
        List<ActionWrapper> listenerList;
        if( typeListenersMap.TryGetValue( typeof( TMessage ), out listenerList ) )
        {
            List<ActionWrapper> listenerListCopy = new List<ActionWrapper>( listenerList ); // Take a copy to avoid mutation exceptions
            foreach( var listener in listenerListCopy ) {
                listener.Call<TParam>( param );
            }
        }
    }

    /// <summary>
    /// The non-generic version of add.
    /// This allow adding of message listeners without knowing the type ahead of time, eg. from an editor.
    /// </summary>
    public static void Add( Type messageType, params Action<object>[] listeners )
    {
        Check.True( messageType.ImplementsGeneric( typeof( IMessage<> ) ) , "Message type must implement IMessage." );

        List<ActionWrapper> listenerList;
        if( false == typeListenersMap.TryGetValue( messageType, out listenerList ) ) {
            listenerList = new List<ActionWrapper>();
            typeListenersMap.Add( messageType, listenerList );
        }

        listenerList.AddRange( listeners.Select( a => ActionWrapper.Create( a ) ) );
    }

    /// <summary>
    /// The generic version of Add. Add a strongly typed message listener.
    /// </summary>
    public static void Add<TMessage, TParam>( params Action<TParam>[] listeners ) where TMessage : IMessage<TParam> {
        Add( typeof( TMessage ), Array.ConvertAll<Action<TParam>, Action<object>>( listeners, a=>a as Action<object> ) );
    }

    /// <summary>
    /// Remove the given listeners associated with the given message type.
    /// </summary>
    public static int Remove( Type messageType, params Action<object>[] listeners )
    {
        Check.True( messageType.ImplementsGeneric( typeof( IMessage<> ) ), "Message type must implement IMessage." );

        List<ActionWrapper> listenerList;
        if( typeListenersMap.TryGetValue( messageType, out listenerList ) ) {
            return listenerList.RemoveAll( l=>listeners.Contains( l.listener ) );
        }
        return 0;
    }

    /// <summary>
    /// The generic version of Remove.
    /// </summary>
    public static int Remove<TMessage, TParam>( params Action<TParam>[] listeners ) where TMessage : IMessage<TParam> {
        return Remove( typeof( TMessage ), Array.ConvertAll<Action<TParam>, Action<object>>( listeners, a=>a as Action<object> ) );
    }

    /// <summary>
    /// Remove the given listeners associated with the given message type.
    /// </summary>
    public static bool RemoveAll( Type messageType )
    {
        Check.True( messageType.ImplementsGeneric( typeof( IMessage<> ) ), "Message type must implement IMessage." );
        if( typeListenersMap.ContainsKey( messageType ) ) {
            typeListenersMap[messageType].Clear();
            return true;
        }
        return false;
    }

    /// <summary>
    /// The generic version of Remove.
    /// </summary>
    public static bool RemoveAll<TMessage,TParam>() where TMessage : IMessage<TParam> {

        return RemoveAll( typeof( TMessage ) );
    }


    /// <summary>
    /// Clear all the listeners. from every message type.
    /// </summary>
    public static void Clear()
    {
        typeListenersMap.Clear();
    }
}
