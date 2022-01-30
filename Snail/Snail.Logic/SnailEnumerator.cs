using System.Collections;

namespace Snail.Logic;

public sealed class SnailEnumerator : IEnumerator<int>
{
    private IState _state;

    public void Reset()
    {
    }

    object IEnumerator.Current => Current;

    public int Current => _state.Current;

    public SnailEnumerator(int[][] array) => _state = new NormalMovement(array);

    public bool MoveNext()
    {
        _state = _state.MoveNext();
        return !_state.IsEndOfSnake;
    }

    public void Dispose()
    {
    }
}