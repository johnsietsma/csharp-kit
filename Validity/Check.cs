using System;
using System.Collections.Generic;

public static class Check
{
    public static void Error( string msg )
    {
        throw new CheckException( msg );
    }

    public static void False( bool test, string message="Not false" )
    {
        True( !test, message );
    }

    public static void True( bool test, string message="Not true" )
    {
        if( !test ) {
            Error( message );
        }
    }

    public static void Equal<T>( T a, T b, string message="" )
    {
        if( !EqualityComparer<T>.Default.Equals(a, b) ) {
            message = message != "" ? message : string.Format( "{0} is not equal to {1}", a, b );
            Error( message );
        }
    }

    public static void NotEqual( int a, int b, string message="" )
    {
        if( a == b ) {
            message = message != "" ? message : string.Format( "{0} is equal to {1}", a, b );
            Error( message );
        }
    }

    public static void GreaterThen( int a, int b, string message="" )
    {
        if( a <= b ) {
            message = message != "" ? message : string.Format( "{0} is not greater then {1}", a, b );
            Error( message );
        }
    }

    public static void GreaterThenOrEqual( int a, int b, string message="" )
    {
        if( a < b ) {
            message = message != "" ? message : string.Format( "{0} is not greater then or equal to {1}", a, b );
            Error( message );
        }
    }

    public static void LessThen( int a, int b, string message="" )
    {
        if( a >= b ) {
            message = message != "" ? message : string.Format( "{0} is not less then {1}", a, b );
            Error( message );
        }
    }

    public static void Null<T>( T obj, string message="Object isn't null" )
    {
        True( EqualityComparer<T>.Default.Equals( obj,default( T ) ), message );
    }

    public static void NotNull<T>( T obj, string message="null object" )
    {
        True( !EqualityComparer<T>.Default.Equals( obj,default( T ) ), message );
    }

    public static void NotEmpty<T>( T[] array, string message="Empty array" )
    {
        True( array == null || array.Length != 0, message );
    }

    public static void NotNullOrEmpty<T>( T[] array, string message="Null or empty array" )
    {
        NotNull( array, message );
        NotEmpty( array, message );
    }

    public static void NotNullOrEmpty( string str, string message="Null or empty string" )
    {
        False( string.IsNullOrEmpty( str ), message );
    }
}
