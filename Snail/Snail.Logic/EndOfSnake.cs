namespace Snail.Logic;

internal class EndOfSnake : IState
{
    public bool IsEndOfSnake => true;

    public int Current => throw new InvalidOperationException("It is not allowed to access Current at the end of the snake.");

    public IState MoveNext() => this;
}