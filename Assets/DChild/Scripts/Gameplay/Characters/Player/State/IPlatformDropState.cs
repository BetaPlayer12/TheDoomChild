namespace DChild.Gameplay.Characters.Players.State
{
    public interface IPlatformDropState
    {
        bool isCrouched { get; }
        bool canPlatformDrop { get; set; }
        bool isDroppingFromPlatform { get; set; }
    }
}