namespace DChild.Gameplay.ArmyBattle
{
    public interface IArmyAbilityInfo
    {
        string groupName { get; }
        int memberCount { get; }
        string abilityDescription { get; }
        ArmyCharacterData GetMember(int index);
        void UseAbility(Army owner, Army oppponent);
    }
}