namespace DChild.Gameplay.Characters.Players.State
{
    public interface ISlideState
    {
        bool isSliding { get; set; }
        bool canSlide { get; set; }
    }
}