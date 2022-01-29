using System.Collections;

namespace Snail.Logic;

public sealed class SnailEnumerator : IEnumerator<int>
{
    private readonly int[][] _array;
    private readonly int _width;
    private int _column;
    private int _row;
    private int _position = -1;

    public void Reset()
    {
        _position = -1;
    }

    object IEnumerator.Current => Current;

    public int Current => _array[_row][_column];

    public SnailEnumerator(int[][] array)
    {
        _array = array;
        _width = _array.Length;
    }

    public bool MoveNext()
    {
        _position++;
        _column = Column();
        _row = Row();

        return _position < _width * _width;
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

    public void Dispose()
    {
    }
}