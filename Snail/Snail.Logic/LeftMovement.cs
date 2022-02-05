namespace Snail.Logic;

internal class LeftMovement : IState
{
    private readonly int[][] _array;

    public LeftMovement(int[][] array) => _array = array;

    public bool IsEndOfSnail => false;
    public int Current => _array[1][0];
    public IState MoveNext() => new EndOfSnail();
}