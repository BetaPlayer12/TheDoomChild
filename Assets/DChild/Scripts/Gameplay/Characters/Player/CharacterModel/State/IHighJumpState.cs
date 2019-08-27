namespace DChild.Gameplay.Characters.Players.State
{
    public interface IHighJumpState
    {
        bool canHighJump { get; set; }
        bool hasJumped { get; set; }
    }
}