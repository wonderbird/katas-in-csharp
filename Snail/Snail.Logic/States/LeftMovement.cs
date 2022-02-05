namespace Snail.Logic.States;

internal class LeftMovement : Movement
{
    private int _column;
    private readonly int _row;

    public LeftMovement(int[][] array) : base(array)
    {
        _column = array.Length - 2;
        _row = array.Length - 1;
    }

    public override int Current => Array[_row][_column];
    public override IState MoveNext()
    {
        if (_column == 0 && _row > 1)
        {
            return new UpMovement(Array);
        }

        if (_column == 0)
        {
            return new EndOfSnail();
        }

        _column--;
        return this;
    }
}