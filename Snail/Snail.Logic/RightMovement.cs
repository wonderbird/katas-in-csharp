namespace Snail.Logic;

internal class RightMovement : IState
{
    private readonly int[][] _array;

    public RightMovement(int[][] array) => _array = array;

    public bool IsEndOfSnail => false;
    public int Current => _array[0][0];
    public IState MoveNext() => new EndOfSnail();
}