namespace DChild.Gameplay.Characters.Players.Attributes
{
    public interface IStrength : IValueChange
    {
        int value { get; }
        int toAttack { get; }
        int bonusAttack { get; }
}

}