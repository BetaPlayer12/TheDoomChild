using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players
{
    public struct CombatStateEventArgs : IEventActionArgs
    {
        public CombatStateEventArgs(bool inCombat) : this()
        {
            this.inCombat = inCombat;
        }
        public bool inCombat { get; }
    }
}