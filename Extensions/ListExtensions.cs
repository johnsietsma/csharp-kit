using System.Collections.Generic;
using System;

public static class ListExtensions
{
    public static void Remove<T> (this IList<T> list, IEnumerable<T> toRemove)
    {
        if (list == null || toRemove == null)
            return;
        
        foreach (T item in toRemove) {
            list.Remove (item);
        }
    }

    public static void Shuffle<T>( this IList<T> list )
    {
        if ( list.Count == 0 )
            return;

        Random rand = new Random();
        for (int i = 0; i < list.Count; i++ ) {
            int j = rand.Next( 0, list.Count );
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    public static T RandomElement<T>( this IList<T> list )
    {
        Random rand = new Random();
        int index = rand.Next( list.Count );
        return list[index];
    }
}
