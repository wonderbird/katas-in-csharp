namespace Snail.Logic.States;

internal class DownMovement : Movement
{
    private readonly int _bottomRow;

    public DownMovement(int[][] array, int column, int row) : base(array, column, row) => _bottomRow = array.Length - 1 - (row-1);

    public override IState MoveNext()
    {
        if (Row == _bottomRow)
        {
            return new LeftMovement(Array, Column - 1, Row);
        }

        Row++;
        return this;
    }
}