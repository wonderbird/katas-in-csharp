namespace Snail.Logic;

internal class DownMovement : IState
{
    private readonly int[][] _array;

    public DownMovement(int[][] array) => _array = array;

    public bool IsEndOfSnail => false;
    public int Current => _array[1][1];

    public IState MoveNext() => new LeftMovement(_array);
}