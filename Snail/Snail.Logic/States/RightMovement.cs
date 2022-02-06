namespace Snail.Logic.States;

internal class RightMovement : Movement
{
    private readonly int _rightMostColumn;
    private readonly int _centerColumn;
    private readonly int _centerRow;

    public RightMovement(int[][] array, int column, int row) : base(array, column, row)
    {
        _rightMostColumn = array.Length - 1 - column;
        _centerColumn = (Array.Length - 1) / 2;
        _centerRow = _centerColumn;
    }

    public override IState MoveNext()
    {
        if (IsAtCenter())
        {
            return new EndOfSnail();
        }

        if (Column == _rightMostColumn)
        {
            return new DownMovement(Array, Column, Row + 1);
        }

        Column++;
        return this;
    }

    private bool IsAtCenter() => Array.Length % 2 == 1 && Column == _centerColumn && Row == _centerRow;
}