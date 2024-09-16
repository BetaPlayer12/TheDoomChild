namespace DChild.Gameplay.ArmyBattle
{
    public interface ISpecialSkillGroup
    {
        bool HasSpecialSkill();

        ArmyCharacterGroup GetCharacterGroup(ArmyCharacterGroup characterGroup);

        SpecialSkill GetSpecialSkill();

    }
}