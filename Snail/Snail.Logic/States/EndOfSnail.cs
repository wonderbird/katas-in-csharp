namespace Snail.Logic.States;

internal class EndOfSnail : IState
{
    public bool IsEndOfSnail => true;

    public int Current =>
        throw new InvalidOperationException("It is not allowed to access Current at the end of the snake.");

    public IState MoveNext() => this;
}