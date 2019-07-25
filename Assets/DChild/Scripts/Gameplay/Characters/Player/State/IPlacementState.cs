namespace DChild.Gameplay.Characters.Players.State
{
    public interface IPlacementState
    {
        bool hasLanded { get; set; }
        bool isGrounded { get; set; }
        bool isFalling { get; set; }
        bool isNearEdge { get; set; }
    }
}