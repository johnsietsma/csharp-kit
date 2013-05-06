using System.Collections.Generic;
using System;
using System.Linq;

public static class MessageSystem
{
    private class ActionWrapper
    {
        private Action<object> action;

        object innerAction;

        public void SetAction<T>( Action<T> genericAction )
        {
            innerAction = genericAction;
            action = o => genericAction( ( T ) o );
        }

        public bool HasAction( object action )
        {
            return innerAction == action;
        }

        public void CallAction<T>( T param )
        {
            action( param );
        }

        public static ActionWrapper Create<T>( Action<T> genericAction )
        {
            Check.NotNull( genericAction );

            ActionWrapper wrapper = new ActionWrapper();
            wrapper.SetAction( genericAction );
            return wrapper;
        }
    }

    static Dictionary<Type, List<ActionWrapper>> listeners = new Dictionary<Type, List<ActionWrapper>>();

    public static void Send<TMessage,TParam>( TParam param ) where TMessage : IMessage<TParam> {
        List<ActionWrapper> listenerList;
        if( listeners.TryGetValue( typeof( TMessage ), out listenerList ) )
        {
            List<ActionWrapper> listenerListCopy = new List<ActionWrapper>( listenerList ); // Take a copy to avoid mutation exceptions
            foreach( var listener in listenerListCopy ) {
                listener.CallAction<TParam>( param );
            }
        }
    }

    public static void Add( Type messageType, params Action<object>[] actions )
    {
        Check.True( ImplementsIMessage( messageType ), "Message type must implement IMessage." );

        List<ActionWrapper> listenerList;
        if( false == listeners.TryGetValue( messageType, out listenerList ) ) {
            listenerList = new List<ActionWrapper>();
            listeners.Add( messageType, listenerList );
        }

        listenerList.AddRange( actions.Select( a => ActionWrapper.Create( a ) ) );
    }

    public static void Remove( Type messageType, params Action<object>[] actions )
    {
        Check.True( ImplementsIMessage( messageType ), "Message type must implement IMessage." );

        List<ActionWrapper> listenerList;
        if( listeners.TryGetValue( messageType, out listenerList ) ) {
            foreach( var action in actions ) {
                listenerList.RemoveAll( w => w.HasAction( action ) );
            }
        }
    }

    public static void Add<TMessage, TParam>( params Action<TParam>[] actions ) where TMessage : IMessage<TParam> {
        List<ActionWrapper> listenerList;
        if( false == listeners.TryGetValue( typeof( TMessage ), out listenerList ) )
        {
            listenerList = new List<ActionWrapper>();
            listeners.Add( typeof( TMessage ), listenerList );
        }

        listenerList.AddRange( actions.Select( a => ActionWrapper.Create( a ) ) );
    }

    public static void Remove<TMessage, TParam>( params Action<TParam>[] actions ) where TMessage : IMessage<TParam> {
        List<ActionWrapper> listenerList;
        if( listeners.TryGetValue( typeof( TMessage ), out listenerList ) )
        {
            foreach( var action in actions ) {
                listenerList.RemoveAll( w => w.HasAction( action ) );
            }
        }
    }

    public static IEnumerable<Action<TParam>> GetListeners<TMessage, TParam>() where TMessage : IMessage<TParam> {
        foreach( Action<TParam> listener in listeners.Cast<Action<TParam>>() )
        {
            yield return listener;
        }
    }

    private static bool ImplementsIMessage( Type type )
    {
        return type.GetInterfaces()
               .Any( i => i.IsGenericType
                     && i.GetGenericTypeDefinition() == typeof( IMessage<> ) );
    }
}
