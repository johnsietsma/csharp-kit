using System;
using System.Collections.Generic;
using System.Linq;

public static class IEnumerableTExtensions
{
    public static string ToStringJoin<T>( this IEnumerable<T> e, string joinString="," )
    {
        List<T> data = new List<T>( e );
        List<string> stringData = data.ConvertAll<string>( d => d.ToString() );
        return String.Join( joinString, stringData.ToArray() );
    }

    public static void ForEach<T>( this IEnumerable<T> enumeration, Action<T> action )
    {
        foreach( T item in enumeration ) {
            action( item );
        }
    }

    public static int Sum<T>( this IEnumerable<T> items, Func<T,int> p )
    {
        int ret = 0;
        items.ForEach( c=>ret+=p( c ) );
        return ret;
    }

    public static IEnumerable<T> Zip<A, B, T>(
        this IEnumerable<A> seqA, IEnumerable<B> seqB, Func<A, B, T> func )
    {
        using ( var iteratorA = seqA.GetEnumerator() )
        using ( var iteratorB = seqB.GetEnumerator() ) {
            while ( iteratorA.MoveNext() && iteratorB.MoveNext() ) {
                yield return func( iteratorA.Current, iteratorB.Current );
            }
        }
    }

    public static IEnumerable<T> Interleave<T>( this IEnumerable<T> first, IEnumerable<T> second )
    {
        var enumerators = new IEnumerator<T>[] { first.GetEnumerator(), second.GetEnumerator() };
        try {
            T[] currentValues;
            do {
                currentValues = enumerators.Where( e => e.MoveNext() ).Select( e => e.Current ).ToArray();
                foreach( T v in currentValues ) yield return v;
            }
            while ( currentValues.Any() );
        }
        finally {
            Array.ForEach( enumerators, e => e.Dispose() );
        }
    }

    // From: http://stackoverflow.com/questions/648196/random-row-from-linq-to-sql/648240#648240
    public static T Random<T>( this IEnumerable<T> source )
    {
        return source.Random( new Random() );
    }

    public static T Random<T>( this IEnumerable<T> source, Random random )
    {
        T randomElement;
        if( !Random( source, random, out randomElement ) ) {
            throw new InvalidOperationException( "Sequence was empty" );
        }
        return randomElement;
    }

    public static T RandomOrDefault<T>( this IEnumerable<T> source )
    {
        return source.RandomOrDefault( new Random() );
    }

    public static T RandomOrDefault<T>( this IEnumerable<T> source, Random random )
    {
        T randomElement;
        Random( source, random, out randomElement );
        return randomElement;
    }

    private static bool Random<T>( IEnumerable<T> source, Random random, out T randomElement )
    {
        randomElement = default( T );
        int count = 0;
        foreach ( T element in source ) {
            count++;
            if ( random.Next( count ) == 0 ) {
                randomElement = element;
            }
        }
        return count>0;
    }
}

