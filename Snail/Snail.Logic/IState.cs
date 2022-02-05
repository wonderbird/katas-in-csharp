namespace Snail.Logic;

internal interface IState
{
    bool IsEndOfSnail { get; }
    int Current { get; }
    IState MoveNext();
}