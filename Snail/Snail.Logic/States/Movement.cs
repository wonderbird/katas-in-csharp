namespace Snail.Logic.States;

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