namespace Snail.Logic;

internal interface IState
{
    bool IsEndOfSnake { get; }
    int Current { get; }
    IState MoveNext();
}