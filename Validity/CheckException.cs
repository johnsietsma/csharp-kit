using System;

/// <summary>
/// A custom exception for use with the Check functions.
/// </summary>
public class CheckException : Exception
{
    public CheckException( string msg ) : base( msg ) {}
}
