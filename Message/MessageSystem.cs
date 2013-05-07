using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// MessageSystem is a type-safe messagin system.
/// </summary>
public static class MessageSystem
{
    /// A dictionary that maps message types to message senders
    private static Dictionary<Type, List<IMessageSender>> typeSendersMap = new Dictionary<Type, List<IMessageSender>>();

    public static IEnumerable<IMessageSender> GetSenders<TMessage, TParam>() where TMessage : IMessage<TParam> {
        Type key =typeof( TMessage );
        if( typeSendersMap.ContainsKey( key ) ) { return typeSendersMap[key]; }
        return Enumerable.Empty<IMessageSender>();
    }


    /// <summary>
    /// Send a message of type TMessage to all senders.
    /// <param name="param">The parameter to send with the message</param>
    /// </summary>
    public static void Send<TMessage,TParam>( TParam param ) where TMessage : IMessage<TParam> {
        List<IMessageSender> senderList;
        if( typeSendersMap.TryGetValue( typeof( TMessage ), out senderList ) )
        {
            List<IMessageSender> senderListCopy = new List<IMessageSender>( senderList ); // Take a copy to avoid mutation exceptions
            foreach( var sender in senderListCopy ) {
                sender.Send<TMessage,TParam>( param );
            }
        }
    }

    public static void Add( Type messageType, params IMessageSender[] senders )
    {
        Check.True( messageType.ImplementsGeneric( typeof( IMessage<> ) ) , "Message type must implement IMessage." );

        List<IMessageSender> senderList;
        if( false == typeSendersMap.TryGetValue( messageType, out senderList ) ) {
            senderList = new List<IMessageSender>();
            typeSendersMap.Add( messageType, senderList );
        }

        senderList.AddRange( senders );
    }

    public static void Add<TMessage>( params IMessageSender[] senders )
    {
        Add( typeof( TMessage ), senders );
    }

    /// <summary>
    /// The non-generic version of add.
    /// This allow adding of message sender without knowing the type ahead of time, eg. from an editor.
    /// </summary>
    public static void Add( Type messageType, params Action<object>[] senders )
    {
        Add( messageType, senders.ConvertAll( s=>MessageSender.Create<object>( messageType, s ) ) );
    }

    /// <summary>
    /// The generic version of Add. Add a strongly typed message sender.
    /// </summary>
    public static void Add<TMessage, TParam>( params Action<TParam>[] senders ) where TMessage : IMessage<TParam> {
        Add( typeof( TMessage ), senders.ConvertAll( s=>s as Action<object> ) );
    }

    /// <summary>
    /// Remove the given senders associated with the given message type.
    /// </summary>
    public static int Remove( Type messageType, params Action<object>[] senders )
    {
        Check.True( messageType.ImplementsGeneric( typeof( IMessage<> ) ), "Message type must implement IMessage." );

        List<IMessageSender> senderList;
        if( typeSendersMap.TryGetValue( messageType, out senderList ) ) {
            return senderList.RemoveAll( l=>senders.Contains( l.Action ) );
        }
        return 0;
    }

    /// <summary>
    /// The generic version of Remove.
    /// </summary>
    public static int Remove<TMessage, TParam>( params Action<TParam>[] senders ) where TMessage : IMessage<TParam> {
        return Remove( typeof( TMessage ), senders.ConvertAll( s=>s as Action<object> ) );
    }

    /// <summary>
    /// Remove the given senders associated with the given message type.
    /// </summary>
    public static bool RemoveAll( Type messageType )
    {
        Check.True( messageType.ImplementsGeneric( typeof( IMessage<> ) ), "Message type must implement IMessage." );
        if( typeSendersMap.ContainsKey( messageType ) ) {
            typeSendersMap[messageType].Clear();
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
    /// Clear all the senders. from every message type.
    /// </summary>
    public static void Clear()
    {
        typeSendersMap.Clear();
    }
}
