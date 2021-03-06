using System;
using System.Collections.Generic;
using System.Linq;

public class MessageHelper : IDisposable
{
    public delegate IMessageSender DecoratorDelegate( IMessageSender sender );

    private List<DecoratorDelegate> decoratorDelegates = new List<DecoratorDelegate>();
    private List<IMessageSender> senders = new List<IMessageSender>();

    ~MessageHelper()
    {
        Clear();
    }

    public void Dispose()
    {
        Clear();
        GC.SuppressFinalize( this );
    }

    public void Clear()
    {
        if( senders==null ) { return; }
        MessageSystem.Remove( senders.ToArray() );
        senders = null;
    }

    public virtual IMessageSender Add( IMessageSender sender )
    {
        foreach( DecoratorDelegate del in decoratorDelegates ) {
            sender = del( sender );
        }
        decoratorDelegates.Clear();
        senders.Add( sender );
        MessageSystem.Add( sender );
        return sender;
    }

    public IMessageSender Add<TMessage,TParam>( Action<TParam> senderAction )
    {
        return Add( MessageSenderBuilder<TMessage>.Create( senderAction ) );
    }

    public MessageHelper AddDecorator( DecoratorDelegate decoratorDelegate )
    {
        decoratorDelegates.Add( decoratorDelegate );
        return this;
    }

    public MessageHelper AddEqualityFilter( object equalsObject )
    {
        decoratorDelegates.Add( MakeEqualityFilterDelegate( equalsObject ) );
        return this;
    }

    public MessageHelper AddInequalityFilter( object notEqualsObject )
    {
        decoratorDelegates.Add( MakeInequalityFilterDelegate( notEqualsObject ) );
        return this;
    }

    public MessageHelper AddFilter( MessageFilterer.MessageFilter filter )
    {
        decoratorDelegates.Add( MakeFilterDelegate( filter ) );
        return this;
    }

    public MessageHelper AddParamFilter<TParam>( Func<TParam,bool> filter )
    {
        decoratorDelegates.Add( MakeParamFilterDelegate<TParam>( filter ) );
        return this;
    }

    public MessageHelper AddLogging( MessageLog log )
    {
        decoratorDelegates.Add( MakeLoggingDelegate( log ) );
        return this;
    }

    public static DecoratorDelegate MakeFilterDelegate( MessageFilterer.MessageFilter filter )
    {
        return ( s )=>new MessageFilterer( s, filter );
    }

    public static DecoratorDelegate MakeParamFilterDelegate<TParam>( Func<TParam,bool> paramFilter )
    {
        MessageFilterer.MessageFilter filter = ( mt,pt,p )=>paramFilter( ( TParam )p );
        return ( s )=>new MessageFilterer( s, filter );
    }

    public static DecoratorDelegate MakeEqualityFilterDelegate( object equalsObject )
    {
        return ( s )=>new MessageFilterer( s, ( mt,pt,p )=>p==equalsObject );
    }

    public static DecoratorDelegate MakeInequalityFilterDelegate( object notEqualsObject )
    {
        return ( s )=>new MessageFilterer( s, ( mt,pt,p )=>p!=notEqualsObject );
    }

    public static DecoratorDelegate MakeLoggingDelegate( MessageLog log )
    {
        return ( s )=>new MessageLogger( s, log );
    }
}
