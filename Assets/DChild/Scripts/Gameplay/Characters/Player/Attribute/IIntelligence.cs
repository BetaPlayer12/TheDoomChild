namespace DChild.Gameplay.Characters.Players.Attributes
{
    public interface IIntelligence : IValueChange
    {
        int value { get; }
        int toMagic { get; }
        int bonusMagic { get; }
        int bonusMagicAttack { get; }
        int bonusMagicDefense { get; }
    }

}