namespace Snail.Logic.States;

internal class DownMovement : Movement
{
    public DownMovement(int[][] array) : base(array)
    {
    }

    public override int Current => Array[1][1];

    public override IState MoveNext() => new LeftMovement(Array);
}