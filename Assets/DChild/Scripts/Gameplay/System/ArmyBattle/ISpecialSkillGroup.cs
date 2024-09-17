namespace DChild.Gameplay.ArmyBattle
{
    public interface ISpecialSkillGroup
    {
        bool HasSpecialSkill(bool hasSkill);

        ArmyCharacterGroup GetCharacterGroup(ArmyCharacterGroup characterGroup);

        SpecialSkill GetSpecialSkill(SpecialSkill specialSkill);

    }
}