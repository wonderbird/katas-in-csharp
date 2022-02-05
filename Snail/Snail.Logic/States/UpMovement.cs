namespace Snail.Logic.States;

internal class UpMovement : Movement
{
    public UpMovement(int[][] array) : base(array)
    {
    }

    public override int Current => Array[1][0];
    public override IState MoveNext() => new RightMovement(Array, 1, 1);
}