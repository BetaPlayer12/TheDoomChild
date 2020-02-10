namespace DChild.Gameplay.Characters.Players.State
{
    public interface IWallStickState
    {
        bool isStickingToWall { get; set; }
        bool isSlidingToWall { get; set; }
        bool isMoving { get; set; }
        bool isFalling { set; }
    }
}