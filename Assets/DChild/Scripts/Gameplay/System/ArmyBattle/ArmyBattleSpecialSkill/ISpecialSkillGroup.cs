namespace DChild.Gameplay.ArmyBattle.SpecialSkills
{
    public interface ISpecialSkillGroup
    {
        int id { get; }
        bool HasSpecialSkill();

        ArmyCharacterGroup GetCharacterGroup();

        SpecialSkill GetSpecialSkill();
    }
}