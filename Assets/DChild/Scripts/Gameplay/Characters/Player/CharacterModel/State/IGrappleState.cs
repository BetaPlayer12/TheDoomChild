namespace DChild.Gameplay.Characters.Players.State
{
    public interface IGrappleState
    {
        bool waitForBehaviour { set; }
        bool canGrapple { get; set; }
        bool isGrappling { get; set; }
    }
}