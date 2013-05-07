using System;

class FilterMessageDecorator : MessageDecorator
{
    public delegate bool MessageFilter( Type messageType, Type paramType, object param );

    public MessageFilter Filter { get; private set; }

    public FilterMessageDecorator( IMessageSender messageSender, MessageFilter filter ) : base( messageSender )
    {
        Filter = filter;
    }

    public override void Send<TMessage,TParam>( TParam param )
    {
        if( Filter( typeof(TMessage), typeof(TParam), param ) ) {
            base.Send<TMessage,TParam>( param );
        }
    }
}