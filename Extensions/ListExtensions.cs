using System.Collections.Generic;
using System;

public static class ListExtensions
{
    static Random random = new Random();

    public static void Swap<T>( this IList<T> list, int firstIndex, int secondIndex )
    {
        Check.NotNull( list );
        CheckInRange( list, firstIndex, "firstIndex" );
        CheckInRange( list, secondIndex, "secondIndex" );

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

    private static void CheckInRange<T>( IList<T> list, int index, string parameterName )
    {
        if( index < 0 || index >= list.Count ) {
            throw new IndexOutOfRangeException(
                string.Format( "Parameter '{0}' out of range: {1} < 0 || {1} >= {2} ", parameterName, index, list.Count )
            );
        }
    }
}
