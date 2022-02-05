namespace Snail.Logic;

internal class Created : IState
{
    public Created(int[][] array)
    {
    }

    public bool IsEndOfSnail => false;

    public int Current =>
        throw new InvalidOperationException("It is not allowed to access Current before MoveNext() has been called.");

    public IState MoveNext()
    {
        return new EndOfSnail();
    }
}