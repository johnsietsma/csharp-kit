using System;
using System.Collections;

public static class IEnumerableExtensions
{
    public static int Count( this IEnumerable e )
    {
        IEnumerator enumerator = e.GetEnumerator();
        int count = 0;
        while( enumerator.MoveNext() ) { count++; }
        return count;
    }
}

