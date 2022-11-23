namespace DChild.Gameplay.Characters.Players.State
{
    public interface IGrabState
    {
        bool isGrabbing { get; set; }
        bool isPulling { get; set; }
        bool isPushing { get; set; }
    }
}


