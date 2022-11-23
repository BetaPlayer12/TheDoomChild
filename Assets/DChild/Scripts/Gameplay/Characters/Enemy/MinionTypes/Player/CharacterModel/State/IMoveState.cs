namespace DChild.Gameplay.Characters.Players.State
{
    public interface IMoveState
    {
        bool isMoving { get; set; }
        bool isJogging { get; set; }
        bool isSprinting { get; set; }
    }
}