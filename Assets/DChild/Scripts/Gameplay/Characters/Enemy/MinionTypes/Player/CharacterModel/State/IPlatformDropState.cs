namespace DChild.Gameplay.Characters.Players.State
{
    public interface IPlatformDropState
    {
        bool canPlatformDrop { get; set; }
        bool isDroppingFromPlatform { get; set; }
    }
}