namespace DChild.Gameplay.ArmyBattle
{
    public interface IAttackingGroup
    {
        int GetTroopCount();
        int GetAttackPower();

        ArmyCharacterGroup GetCharacterGroup();
        DamageType GetDamageType();
    }
}