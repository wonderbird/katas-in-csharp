namespace Snail.Logic
{
    public class SnailSolution
    {
        public static int[] Snail(int[][] array)
        {
            var n = array.Length;
            var result = new List<int>();

            for (var index = 0; index < n * n; index++)
            {
                result.Add(array[Row(index, n)][Column(index, n)]);
            }

            return result.ToArray();
        }

        private static int Column(int index, int size)
        {
            if (index == 0)
            {
                return 0;
            }

            return index == 1 ? 1 : index == 2 ? 1 : 0;
        }

        private static int Row(int index, int size)
        {
            if (index == 0)
            {
                return 0;
            }

            return index == 1 ? 0 : index == 2 ? 1 : 1;
        }
    }
}