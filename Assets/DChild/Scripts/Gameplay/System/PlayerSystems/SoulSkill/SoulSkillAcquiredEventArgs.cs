using Holysoft.Event;

namespace DChild.Gameplay.SoulSkills
{
    public struct SoulSkillAcquiredEventArgs : IEventActionArgs
    {
        public SoulSkillAcquiredEventArgs(int ID) : this()
        {
            this.ID = ID;
        }

        public int ID { get; }
    }
}
