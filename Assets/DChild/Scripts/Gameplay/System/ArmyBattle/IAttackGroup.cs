namespace DChild.Gameplay.ArmyBattle
{
    public interface IAttackGroup
    {
        string groupName { get; }
        UnitType attackType { get; }
        int memberCount { get; }
        bool isUsingCharactersForPower { get; }
        int GetAttackPower();

        ArmyCharacterGroup GetCharacterGroup();
        DamageType GetDamageType();
    }
}