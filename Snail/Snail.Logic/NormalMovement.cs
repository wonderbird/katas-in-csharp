namespace Snail.Logic;

internal class NormalMovement : IState
{
    private readonly int[][] _array;
    private readonly int _width;
    private int _position;
    private int _column;
    private int _row;

    public NormalMovement(int[][] array, int position = -1)
    {
        _array = array;
        _width = _array.Length;
        _position = position;
    }

    public bool IsEndOfSnake => false;
    public int Current => _array[_row][_column];

    public IState MoveNext()
    {
        _position++;
        _column = Column();
        _row = Row();

        if (_position < _width * _width)
        {
            return this;
        }

        return new EndOfSnake();
    }

    private int Column()
    {
        return _position switch
        {
            0 => 0,
            1 => 1,
            2 => 1,
            _ => 0
        };
    }

    private int Row()
    {
        return _position switch
        {
            0 => 0,
            1 => 0,
            2 => 1,
            _ => 1
        };
    }
}