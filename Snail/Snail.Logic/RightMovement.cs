namespace Snail.Logic;

internal class RightMovement : IState
{
    private readonly int[][] _array;
    private int _column;
    private readonly int _rightMostColumn;

    public RightMovement(int[][] array)
    {
        _array = array;
        _rightMostColumn = array.Length - 1;
    }

    public bool IsEndOfSnail => false;
    public int Current => _array[0][_column];
    public IState MoveNext()
    {
        if (_array.Length == 1)
        {
            return new EndOfSnail();
        }

        if (_column == _rightMostColumn)
        {
            return new DownMovement(_array);
        }

        _column++;
        return this;
    }
}