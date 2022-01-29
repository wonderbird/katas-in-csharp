using System.Collections;

namespace Snail.Logic
{
    public class SnailSolution : IEnumerable<int>
    {
        private readonly int[][] _array;

        private SnailSolution(int[][] array) => _array = array;

        public static int[] Snail(int[][] array) => new SnailSolution(array).ToArray();

        public IEnumerator<int> GetEnumerator() => new SnailEnumerator(_array);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}