namespace DChild.Gameplay.ArmyBattle
{
    public interface IArmyAbilityEffect
    {
        void ApplyEffect(Army owner, Army opponent);
    }
}