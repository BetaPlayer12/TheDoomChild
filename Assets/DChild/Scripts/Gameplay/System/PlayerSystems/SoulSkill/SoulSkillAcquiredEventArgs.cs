using DChild.Gameplay.Characters.Players.SoulSkills;
using Holysoft.Event;

namespace DChild.Gameplay.SoulSkills
{
    public struct SoulSkillAcquiredEventArgs : IEventActionArgs
    {
        public SoulSkillAcquiredEventArgs(SoulSkill skill) : this()
        {
            this.skill = skill;
        }

        private SoulSkill skill { get; }

        public int ID => skill.id;
    }
}
