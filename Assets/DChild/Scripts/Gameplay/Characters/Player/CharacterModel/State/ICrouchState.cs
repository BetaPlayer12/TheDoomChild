namespace DChild.Gameplay.Characters.Players.State
{
    public interface ICrouchState
    {
        bool isCrouched { get; set; }
        bool isMoving { get; set; }
    }
}