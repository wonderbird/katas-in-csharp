using System.Collections;

namespace Snail.Logic;

public class Snail : IEnumerable<int>
{
    private readonly int[][] _array;

    public Snail(int[][] array) => _array = array;

    public IEnumerator<int> GetEnumerator() => new SnailEnumerator(_array);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}