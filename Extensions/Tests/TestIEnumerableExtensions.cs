using NUnit.Framework;
using System.Linq;

[TestFixture]
public class TestIEnumerableExtensions
{
    [Test]
    public void TestInterleave()
    {
        int[] seq1 = new int[] { 1, 3, 5, 6 };
        int[] seq2 = new int[] { 2, 4 };

        int[] seq3 = seq1.Interleave( seq2 ).ToArray();

        int[] res = new int[] { 1, 2, 3, 4, 5, 6 };
        Assert.AreEqual( seq3, res );
    }
}
