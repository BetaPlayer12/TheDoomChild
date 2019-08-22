using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players
{
    public struct StatValueEventArgs : IEventActionArgs
    {
        public StatValueEventArgs(PlayerStat stat, int value) : this()
        {
            this.stat = stat;
            this.value = value;
        }

        public PlayerStat stat { get; }
        public int value { get; }
    }
}