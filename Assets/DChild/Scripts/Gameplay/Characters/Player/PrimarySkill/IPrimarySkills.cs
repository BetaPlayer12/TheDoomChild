using Holysoft.Event;
using DChild.Gameplay.Characters.Players;

namespace DChild.Gameplay.Characters.Players
{
    public interface IPrimarySkills
    {
        event EventAction<PrimarySkillUpdateEventArgs> SkillUpdate;
    }
}
