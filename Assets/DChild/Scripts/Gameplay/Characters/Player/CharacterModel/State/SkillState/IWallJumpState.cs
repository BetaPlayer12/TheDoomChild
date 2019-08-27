namespace DChild.Gameplay.Characters.Players.State
{
    public interface IWallJumpState
    {
        bool canWallJump { get; set; }
        bool isStickingToWall { get; set; }
        bool waitForBehaviour { get; set; }
    }
}