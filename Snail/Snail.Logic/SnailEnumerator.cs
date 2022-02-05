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