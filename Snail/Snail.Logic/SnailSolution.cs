namespace Snail.Logic
{
    public class SnailSolution
    {
        public static int[] Snail(int[][] array) => SnailEnumerator.Enumerate(array).ToArray();
    }
}