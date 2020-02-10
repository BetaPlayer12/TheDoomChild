using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.Players;

namespace DChild.Gameplay.Characters.Players
{
    public interface IPrimarySkills
    {
        event EventAction<PrimarySkillUpdateEventArgs> SkillUpdate;
        bool IsEnabled(PrimarySkill skill);
    }
}
