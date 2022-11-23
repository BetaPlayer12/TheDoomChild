using DChild.Gameplay.Characters.Players.State;
using DChild.Inputs;
using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public struct CombatEventArgs : IEventActionArgs
    {
        public CombatEventArgs(DirectionalInput directionalInput)
        {
            this.directionalInput = directionalInput;
        }

        public DirectionalInput directionalInput { get; }
    }
}