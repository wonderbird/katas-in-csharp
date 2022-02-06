namespace Snail.Logic.States;

internal class UpMovement : Movement
{
    private readonly int _column;
    private int _row;
    private readonly int _topMostRow;

    public UpMovement(int[][] array, int column, int row) : base(array)
    {
        _column = column;
        _row = row;

        _topMostRow = column + 1;
    }

    public override int Current => Array[_row][_column];
    public override IState MoveNext()
    {
        if (_row == _topMostRow)
        {
            return new RightMovement(Array, _column + 1, _row);
        }

        _row--;
        return this;
    }
}