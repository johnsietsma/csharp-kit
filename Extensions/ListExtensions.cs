using System.Collections.Generic;
using System;

public static class ListExtensions
{
    static Random random = new Random();

    public static void Swap<T>( this IList<T> list, int firstIndex, int secondIndex )
    {
        Check.NotNull( list );
        Check.True( firstIndex >= 0 && firstIndex < list.Count );
        Check.True( secondIndex >= 0 && secondIndex < list.Count );
        if ( firstIndex == secondIndex ) {
            return;
        }

        T temp = list[firstIndex];
        list[firstIndex] = list[secondIndex];
        list[secondIndex] = temp;
    }

    public static void Remove<T>(this IList<T> list, IEnumerable<T> toRemove)
    {
        Check.NotNull( list );
        
        foreach (T item in toRemove) {
            list.Remove (item);
        }
    }

    public static void Shuffle<T>( this IList<T> list )
    {
        if ( list.Count == 0 )
        return;

        for (int i = 0; i < list.Count; i++ ) {
            int j = random.Next( 0, list.Count );
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
