namespace Snail.Logic.States;

internal class UpMovement : Movement
{
    private readonly int _topMostRow;

    public UpMovement(int[][] array, int column, int row) : base(array, column, row) => _topMostRow = column + 1;

    public override IState MoveNext()
    {
        if (Row == _topMostRow)
        {
            return new RightMovement(Array, Column + 1, Row);
        }

        Row--;
        return this;
    }
}