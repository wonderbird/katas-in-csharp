using System.Collections;

namespace Snail.Logic;

public static class SnailSolution
{
    public static int[] Snail(int[][] array) => new Snail(array).ToArray();
}

public class Snail : IEnumerable<int>
{
    private readonly int[][] _array;

    public Snail(int[][] array) => _array = array;

    public IEnumerator<int> GetEnumerator() => new SnailEnumerator(_array);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public sealed class SnailEnumerator : IEnumerator<int>
{
    private IState _state;

    public void Reset()
    {
    }

    object IEnumerator.Current => Current;

    public int Current => _state.Current;

    public SnailEnumerator(int[][] array) => _state = new Created(array);

    public bool MoveNext()
    {
        _state = _state.MoveNext();
        return !_state.IsEndOfSnail;
    }

    public void Dispose()
    {
    }
}

internal interface IState
{
    bool IsEndOfSnail { get; }
    int Current { get; }
    IState MoveNext();
}

internal class Created : IState
{
    private readonly int[][] _array;

    public Created(int[][] array) => _array = array;

    public bool IsEndOfSnail => false;

    public int Current =>
        throw new InvalidOperationException("It is not allowed to access Current before MoveNext() has been called.");

    public IState MoveNext()
    {
        if (_array.Length > 0)
        {
            return new RightMovement(_array, 0, 0);
        }

        return new EndOfSnail();
    }
}

internal class EndOfSnail : IState
{
    public bool IsEndOfSnail => true;

    public int Current =>
        throw new InvalidOperationException("It is not allowed to access Current at the end of the snake.");

    public IState MoveNext() => this;
}

internal abstract class Movement : IState
{
    protected readonly int[][] Array;
    protected int Column;
    protected int Row;

    protected Movement(int[][] array, int column, int row)
    {
        Array = array;
        Column = column;
        Row = row;
    }

    public bool IsEndOfSnail => false;
    public int Current => Array[Row][Column];
    public abstract IState MoveNext();
}

internal class RightMovement : Movement
{
    private readonly int _rightMostColumn;
    private readonly int _centerColumn;
    private readonly int _centerRow;

    public RightMovement(int[][] array, int column, int row) : base(array, column, row)
    {
        _rightMostColumn = array.Length - 1 - column;
        _centerColumn = (Array.Length - 1) / 2;
        _centerRow = _centerColumn;
    }

    public override IState MoveNext()
    {
        if (IsAtCenter())
        {
            return new EndOfSnail();
        }

        if (Column == _rightMostColumn)
        {
            return new DownMovement(Array, Column, Row + 1);
        }

        Column++;
        return this;
    }

    private bool IsAtCenter() => Array.Length % 2 == 1 && Column == _centerColumn && Row == _centerRow;
}

internal class DownMovement : Movement
{
    private readonly int _bottomRow;

    public DownMovement(int[][] array, int column, int row) : base(array, column, row) => _bottomRow = array.Length - 1 - (row-1);

    public override IState MoveNext()
    {
        if (Row == _bottomRow)
        {
            return new LeftMovement(Array, Column - 1, Row);
        }

        Row++;
        return this;
    }
}

internal class LeftMovement : Movement
{
    private readonly int _terminatingColumn;
    private readonly int _terminatingRow;
    private readonly int _leftMostColumn;

    public LeftMovement(int[][] array, int column, int row) : base(array, column, row)
    {
        _leftMostColumn = Array.Length - 2 - column;
        _terminatingColumn = Array.Length / 2 - 1;
        _terminatingRow = _terminatingColumn + 1;
    }

    public override IState MoveNext()
    {
        if (IsAtTerminatingPosition())
        {
            return new EndOfSnail();
        }

        if (Column == _leftMostColumn)
        {
            return new UpMovement(Array, Column, Row - 1);
        }

        Column--;
        return this;
    }

    private bool IsAtTerminatingPosition() => Array.Length % 2 == 0 && Column == _terminatingColumn && Row == _terminatingRow;
}

internal class UpMovement : Movement
{
    private readonly int _topMostRow;

    public UpMovement(int[][] array, int column, int row) : base(array, column, row) => _topMostRow = column + 1;

    public override IState MoveNext()
    {
        if (Row == _topMostRow)
        {
            return new RightMovement(Array, Column + 1, Row);
        }

        Row--;
        return this;
    }
}
