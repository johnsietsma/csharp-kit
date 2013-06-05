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

    // ----- Add Functions ----
    public static IMessageSender Add( IMessageSender sender )
    {
        Type messageType = sender.MessageType;
        Check.True( messageType.ImplementsGeneric( typeof( IMessage<> ) ) , "Message type must implement IMessage." );

        if( !typeSendersMap.ContainsKey(messageType) ) {
            typeSendersMap.Add( messageType, new List<IMessageSender>() );
        }
        typeSendersMap[messageType].Add( sender );
        return sender;
    }

    public static IMessageSender[] Add( params IMessageSender[] senders )
    {
        Array.ForEach( senders, s=>Add(s) );
        return senders;
    }

    public static IMessageSender[] Add<TParam>( Type messageType, params Action<TParam>[] senders )
    {
        IMessageSender[] msgSenders = senders.ConvertAll( s=>MessageSender.Create<TParam>( messageType, s ) );
        Add( msgSenders );
        return msgSenders;
    }

    public static IMessageSender[] Add<TMessage, TParam>( params Action<TParam>[] senderActions )  where TMessage : IMessage<TParam> {
        Type messageType = typeof(TMessage);
        return Add( senderActions.ConvertAll( s=>MessageSender.Create<TParam>( messageType, s ) ) );
    }

    // ----- Remove Functions ----
    public static int Remove( Type messageType, object senderAction )
    {
        Check.True( messageType.ImplementsGeneric( typeof( IMessage<> ) ), "Message type must implement IMessage." );

        if( typeSendersMap.ContainsKey(messageType) ) {
            return typeSendersMap[messageType].RemoveAll( l=>senderAction==l.OriginalAction );
        }
        return 0;
    }

    public static int Remove<TParam>( Type messageType, params Action<TParam>[] senders )
    {
        Check.True( messageType.ImplementsGeneric( typeof( IMessage<> ) ), "Message type must implement IMessage." );

        if( typeSendersMap.ContainsKey(messageType) ) {
            return typeSendersMap[messageType].RemoveAll( l=>senders.Contains( l.OriginalAction ) );
        }
        return 0;
    }

    public static int Remove<TMessage, TParam>( params Action<TParam>[] senders )  where TMessage : IMessage<TParam> {
        return Remove( typeof( TMessage ), senders );
    }

    public static void Remove( IMessageSender[] senders )
    {
        Array.ForEach<IMessageSender>( senders, s=>Remove( s.MessageType, s.OriginalAction ) );
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
    public static bool RemoveAll<TMessage>() {

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
