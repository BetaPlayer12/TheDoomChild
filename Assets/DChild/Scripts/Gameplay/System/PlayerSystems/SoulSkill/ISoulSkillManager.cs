namespace DChild.Gameplay.SoulSkills
{
    public interface ISoulSkillManager
    {
        void AllowSoulSkillActivation(bool canActivateSoulSkill);
        void ForceAllowSoulSkillActivation(bool v);
    }
}
