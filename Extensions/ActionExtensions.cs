using System;

public static class ActionExtensions
{
    public static Action<object> ToObjectAction<T>( this Action<T> myActionT )
    {
        if ( myActionT == null ) { return null; }
        else { return new Action<object>( o => myActionT( (T)o ) ); }
    }
}