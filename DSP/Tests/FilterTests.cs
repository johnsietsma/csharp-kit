using System;
using NUnit.Framework;

[TestFixture]
public class FilterTests
{
    [Test]
    public void TestLowPass()
    {
        float[] rawSeries = new float[] { 1,1,1,0,0 };
        float[] filteredSeries = new float[rawSeries.Length];

        filteredSeries[0] = rawSeries[0];
        float smooth = 0.8f;
        for( int i=1; i<rawSeries.Length; i++ ) {
            filteredSeries[i] = Filter.LowPass( filteredSeries[i-1], rawSeries[i], smooth );
        }

        Console.Out.Write( "Raw: " );
        DumpArray( rawSeries );
        Console.Out.WriteLine( "" );

        Console.Out.Write( "Smooth: " );
        DumpArray( filteredSeries );
        Console.Out.WriteLine( "" );

        Assert.AreEqual( 0.04f, filteredSeries[4], 0.001f );
    }

    [Test]
    public void TestHighPass()
    {
        float[] rawSeries = new float[] { 1,1,1,0,0 };
        float[] filteredSeries = new float[rawSeries.Length ];

        filteredSeries[0] = rawSeries[0];
        float smooth = 0.1f;
        for( int i=1; i<rawSeries.Length; i++ ) {
            filteredSeries[i] = Filter.HighPass( filteredSeries[i - 1], rawSeries[i], rawSeries[i - 1], smooth );
        }

        Console.Out.Write( "Raw: " );
        DumpArray( rawSeries );
        Console.Out.WriteLine( "" );

        Console.Out.Write( "Smooth: " );
        DumpArray( filteredSeries );
        Console.Out.WriteLine( "" );

        Assert.AreEqual( -0.0099f, filteredSeries[4], 0.001f );
    }

    private void DumpArray( float[] floatArray )
    {
        Array.ForEach( floatArray, f => Console.Out.Write( f + "," ) );
    }
}

