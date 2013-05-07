using System;

/// <summary>
/// An interface for calling a message in the MessageSystem.
/// </summary>
public interface IMessageSender
{
    Type MessageType { get; }
    Type ParamType { get; }
    object OriginalAction { get; }

    Action<T> GetAction<T>();

    void Send<TMessage,TParam>( TParam param );
}
