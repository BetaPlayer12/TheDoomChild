namespace DChild.Gameplay.ArmyBattle
{
    public interface ISpecialSkillGroup
    {
        string groupName { get; }
        int memberCount { get; }
        string abilityDescription { get; }
        bool hasSpecialSkill { get; }
        ArmyCharacterData GetMember(int index);
        void UseAbility(Army owner, Army oppponent);

        ArmyCharacterGroup GetCharacterGroup(ArmyCharacterGroup characterGroup);

        SpecialSkill GetSpecialSkill();

    }
}