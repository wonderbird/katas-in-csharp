namespace Snail.Logic.States;

internal class UpMovement : Movement
{
    private int _row;

    public UpMovement(int[][] array, int row) : base(array) => _row = row;

    public override int Current => Array[_row][0];
    public override IState MoveNext()
    {
        if (_row == 1)
        {
            return new RightMovement(Array, 1, _row);
        }

        _row--;
        return this;
    }
}