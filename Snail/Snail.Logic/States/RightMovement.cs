namespace Snail.Logic.States;

internal class RightMovement : Movement
{
    private readonly int _rightMostColumn;
    private int _column;

    public RightMovement(int[][] array) : base(array) => _rightMostColumn = array.Length - 1;

    public override int Current => Array[0][_column];

    public override IState MoveNext()
    {
        if (Array.Length == 1)
        {
            return new EndOfSnail();
        }

        if (_column == _rightMostColumn)
        {
            return new DownMovement(Array);
        }

        _column++;
        return this;
    }
}
