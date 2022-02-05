namespace Snail.Logic.States;

internal class DownMovement : Movement
{
    private readonly int _column;
    private readonly int _bottomRow;
    private int _row;

    public DownMovement(int[][] array, int column, int row) : base(array)
    {
        _column = column;
        _row = row;
        _bottomRow = array.Length - 1 - (row-1);
    }

    public override int Current => Array[_row][_column];

    public override IState MoveNext()
    {
        if (_row == _bottomRow)
        {
            return new LeftMovement(Array, _column - 1, _row);
        }

        _row++;
        return this;
    }
}