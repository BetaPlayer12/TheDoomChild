using DChild.Gameplay.Characters.Players.State;
using DChild.Inputs;
using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public struct CombatEventArgs : IEventActionArgs
    {
        public CombatEventArgs(IFacing facing)
        {
            this.facing = facing;
        }

        public IFacing facing { get; }
    }
}