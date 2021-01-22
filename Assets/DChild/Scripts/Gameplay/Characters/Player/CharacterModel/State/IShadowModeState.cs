namespace DChild.Gameplay.Characters.Players.State
{
    public interface IShadowModeState
    {
        bool isInShadowMode { get; set; }
        bool waitForBehaviour { get; set; }
    }
}