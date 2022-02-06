namespace Snail.Logic.States;

internal class LeftMovement : Movement
{
    private readonly int _terminatingColumn;
    private readonly int _terminatingRow;
    private readonly int _leftMostColumn;

    public LeftMovement(int[][] array, int column, int row) : base(array, column, row)
    {
        _leftMostColumn = Array.Length - 2 - column;
        _terminatingColumn = Array.Length / 2 - 1;
        _terminatingRow = _terminatingColumn + 1;
    }

    public override IState MoveNext()
    {
        if (IsAtTerminatingPosition())
        {
            return new EndOfSnail();
        }

        if (Column == _leftMostColumn)
        {
            return new UpMovement(Array, Column, Row - 1);
        }

        Column--;
        return this;
    }

    private bool IsAtTerminatingPosition() => Array.Length % 2 == 0 && Column == _terminatingColumn && Row == _terminatingRow;
}