namespace Snail.Logic.States;

internal abstract class Movement : IState
{
    protected readonly int[][] Array;

    protected Movement(int[][] array) => Array = array;

    public bool IsEndOfSnail => false;
    public abstract int Current { get; }
    public abstract IState MoveNext();
}