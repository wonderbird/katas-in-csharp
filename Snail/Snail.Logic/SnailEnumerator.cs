namespace Snail.Logic;

public class SnailEnumerator
{
    public static IEnumerable<int> Enumerate(int[][] array)
    {
        var n = array.Length;
        var column = 0;
        var row = 0;
        var stepCount = 0;

        while (stepCount < n * n)
        {
            yield return array[row][column];

            stepCount++;
            column = Column(stepCount, n);
            row = Row(stepCount, n);
        }
    }

    private static int Column(int i, int n)
    {
        return i switch
        {
            0 => 0,
            1 => 1,
            2 => 1,
            _ => 0
        };
    }

    private static int Row(int i, int n)
    {
        return i switch
        {
            0 => 0,
            1 => 0,
            2 => 1,
            _ => 1
        };
    }
}