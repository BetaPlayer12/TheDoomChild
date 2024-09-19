namespace DChild.Gameplay.ArmyBattle.SpecialSkills
{
    public interface ISpecialSkillGroup
    {
        bool HasSpecialSkill();

        ArmyCharacterGroup GetCharacterGroup();

        SpecialSkill GetSpecialSkill();
    }
}