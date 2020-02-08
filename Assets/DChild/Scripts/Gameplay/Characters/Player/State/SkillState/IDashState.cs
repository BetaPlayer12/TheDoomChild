namespace DChild.Gameplay.Characters.Players.State
{
    public interface IDashState
    {
        bool isDashing { get; set; }
        bool canDash { get; set; }
    }
}