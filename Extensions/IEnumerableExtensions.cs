using System;
using System.Collections.Generic;
using System.Linq;

public static class IEnumerableExtensions
{
    public static int Count<T>( this IEnumerable<T> e )
    {
        return new List<T>( e ).Count;
    }

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

    public static int Sum<T>( this IEnumerable<T> items, Func<T,int> p ) {
        int ret = 0;
        items.ForEach( c=>ret+=p(c));
        return ret;
    }

    public static IEnumerable<T> Zip<A, B, T>(
        this IEnumerable<A> seqA, IEnumerable<B> seqB, Func<A, B, T> func)
    {
        using (var iteratorA = seqA.GetEnumerator())
            using (var iteratorB = seqB.GetEnumerator())
        {
            while (iteratorA.MoveNext() && iteratorB.MoveNext())
            {
                yield return func(iteratorA.Current, iteratorB.Current);
            }
        }
    }

    // Not thread-safe.
    // Taken from 'http://stackoverflow.com/questions/273313/randomize-a-listt-in-c-sharp'.
    public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
    {
        Random rnd = new Random();
        return source.OrderBy<T, int>( e => rnd.Next() );
    }
}

