namespace DChild.Gameplay.Characters.Players.State
{
    public interface IFlinchState
    {
        bool isFlinching { get; set; }
        bool canFlinch { get; set; }
    }
}