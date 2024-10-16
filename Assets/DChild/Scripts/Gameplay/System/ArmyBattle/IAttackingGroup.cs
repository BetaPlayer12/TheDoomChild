namespace DChild.Gameplay.ArmyBattle
{
    public interface IAttackingGroup
    {
        int id { get; }
        int GetTroopCount();
        int GetAttackPower();

        ArmyCharacterGroup GetCharacterGroup();
        DamageType GetDamageType();
    }
}