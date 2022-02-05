namespace Snail.Logic.States;

internal class DownMovement : Movement
{
    private readonly int _column;
    private readonly int _bottomRow;
    private int _row = 1;

    public DownMovement(int[][] array, int column) : base(array)
    {
        _column = column;
        _bottomRow = array.Length - 1;
    }

    public override int Current => Array[_row][_column];

    public override IState MoveNext()
    {
        if (_row == _bottomRow)
        {
            return new LeftMovement(Array);
        }

        _row++;
        return this;
    }
}