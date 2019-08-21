namespace DChild.Gameplay.Characters.Players.State
{
    public interface IPlacementState
    {
        bool isGrounded { get; set; }
        bool isFalling { get; set; }
        bool isNearEdge { get; set; }
    }
}