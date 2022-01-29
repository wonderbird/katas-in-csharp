namespace Snail.Logic
{
    public class SnailSolution
    {
        public static int[] Snail(int[][] array) => new SnailEnumerator(array).Enumerate().ToArray();
    }
}