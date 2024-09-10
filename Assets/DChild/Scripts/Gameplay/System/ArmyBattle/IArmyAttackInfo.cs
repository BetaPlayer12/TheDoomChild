namespace DChild.Gameplay.ArmyBattle
{
    public interface IArmyAttackInfo
    {
        string groupName { get; }
        UnitType attackType { get; }
        int memberCount { get; }
        bool isUsingCharactersForPower { get; }
        ArmyCharacterData GetMember(int index);
        int GetTotalAttackPower();
    }
}