namespace Snail.Logic;

public class SnailEnumerator
{
    private readonly int[][] _array;
    private readonly int _width;
    private int _column;
    private int _row;
    private int _stepCount;

    public int Current => _array[_row][_column];

    public SnailEnumerator(int[][] array)
    {
        _array = array;
        _width = _array.Length;
    }

    public IEnumerable<int> Enumerate()
    {
        while (_stepCount < _width * _width)
        {
            yield return Current;

            _stepCount++;
            _column = Column();
            _row = Row();
        }
    }

    private int Column()
    {
        return _stepCount switch
        {
            0 => 0,
            1 => 1,
            2 => 1,
            _ => 0
        };
    }

    private int Row()
    {
        return _stepCount switch
        {
            0 => 0,
            1 => 0,
            2 => 1,
            _ => 1
        };
    }
}