namespace DChild.Gameplay.Characters.Players.State
{
    public interface IDoubleJumpState
    {
        bool canDoubleJump { get; set; }
        bool hasDoubleJumped { get; set; }
    }
}