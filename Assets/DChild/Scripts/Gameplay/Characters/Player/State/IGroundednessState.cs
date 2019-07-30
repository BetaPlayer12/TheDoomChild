namespace DChild.Gameplay.Characters.Players.State
{
    public interface IGroundednessState
    {
        bool isGrounded { get; set; }
        bool isFalling { get; set; }
        bool isNearEdge { get; set; }
        bool waitForBehaviour { set; }
    }
}