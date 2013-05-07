using System;

/// <summary>
/// An interface for calling a message in the MessageSystem.
/// </summary>
public interface IMessageSender
{
    Action<object> Action { get; }
    Type MessageType { get; }
    Type ParamType { get; }

    void Send<TMessage,TParam>( TParam param );
}
