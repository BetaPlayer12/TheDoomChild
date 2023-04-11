using DChild.Gameplay.Characters.Players.SoulSkills;
using Holysoft.Event;

namespace DChild.Gameplay.SoulSkills
{
    public struct SoulSkillAcquiredEventArgs : IEventActionArgs
    {
        public SoulSkillAcquiredEventArgs(SoulSkill skill) : this()
        {
            this.SoulSKill = skill;
        }

        public SoulSkill SoulSKill { get; }

        public int ID => SoulSKill.id;
    }
}
