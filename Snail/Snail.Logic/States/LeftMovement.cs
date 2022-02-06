namespace Snail.Logic.States;

internal class LeftMovement : Movement
{
    private int _column;
    private readonly int _row;
    private readonly int _terminatingColumn;
    private readonly int _terminatingRow;
    private readonly int _leftMostColumn;

    public LeftMovement(int[][] array, int column, int row) : base(array)
    {
        _column = column;
        _row = row;

        _leftMostColumn = Array.Length - 2 - column;
        _terminatingColumn = Array.Length / 2 - 1;
        _terminatingRow = _terminatingColumn + 1;
    }

    public override int Current => Array[_row][_column];
    public override IState MoveNext()
    {
        if (IsAtTerminatingPosition())
        {
            return new EndOfSnail();
        }

        if (_column == _leftMostColumn && _row > 1)
        {
            return new UpMovement(Array, _column, _row - 1);
        }
        _column--;
        return this;
    }

    private bool IsAtTerminatingPosition() => Array.Length % 2 == 0 && _column == _terminatingColumn && _row == _terminatingRow;
}