namespace DChild.Gameplay.Characters.Players.Attributes
{
    public interface ILuck : IValueChange
    {
        float critChance { get; }
        float statusChance { get; }
    }
}