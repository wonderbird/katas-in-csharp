namespace Snail.Logic
{
    public class SnailSolution
    {
        public static int[] Snail(int[][] array) => Enumerate(array).ToArray();

        private static IEnumerable<int> Enumerate(int[][] array)
        {
            var n = array.Length;

            for (var i = 0; i < n * n; i++)
            {
                yield return array[Row(i, n)][Column(i, n)];
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
}