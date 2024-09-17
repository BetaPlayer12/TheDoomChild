namespace DChild.Gameplay.ArmyBattle
{
    public interface ISpecialSkillGroup
    {
        bool HasSpecialSkill();

        ArmyCharacterGroup GetCharacterGroup();

        SpecialSkill GetSpecialSkill();

    }
}