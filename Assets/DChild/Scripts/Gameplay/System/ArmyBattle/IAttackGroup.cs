namespace DChild.Gameplay.ArmyBattle
{
    public interface IAttackGroup
    {
        int GetTroopCount();
        int GetAttackPower();

        ArmyCharacterGroup GetCharacterGroup();
        DamageType GetDamageType();
    }
}