namespace Snail.Logic.States;

internal class LeftMovement : Movement
{
    public LeftMovement(int[][] array) : base(array)
    {
    }

    public override int Current => Array[1][0];
    public override IState MoveNext() => new EndOfSnail();
}