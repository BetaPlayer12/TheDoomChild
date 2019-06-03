namespace DChild.Gameplay.Characters.Players.Attributes
{
    public interface IVitality : IValueChange
    {

        int value { get; }
        int toHealth { get; }
        int bonusHealth { get; }
        int bonusDefense { get; }
        int bonusMagicDefense { get; }
    }
}