namespace Snail.Logic.States;

internal class RightMovement : Movement
{
    private readonly int _rightMostColumn;
    private int _column;
    private readonly int _row;
    private int _centerColumn;
    private int _centerRow;

    public RightMovement(int[][] array, int column, int row) : base(array)
    {
        _column = column;
        _row = row;

        _rightMostColumn = array.Length - 1 - column;
        _centerColumn = (Array.Length - 1) / 2;
        _centerRow = _centerColumn;
    }

    public override int Current => Array[_row][_column];

    public override IState MoveNext()
    {
        if (IsAtCenter())
        {
            return new EndOfSnail();
        }

        if (_column == _rightMostColumn)
        {
            return new DownMovement(Array, _column);
        }

        _column++;
        return this;
    }

    private bool IsAtCenter() => Array.Length % 2 == 1 && _column == _centerColumn && _row == _centerRow;
}