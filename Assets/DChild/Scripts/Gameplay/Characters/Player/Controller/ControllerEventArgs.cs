using DChild.Inputs;
using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public struct ControllerEventArgs : IEventActionArgs
    {
        public ControllerEventArgs(PlayerInput input)
        {
            this.input = input;
        }

        public PlayerInput input { get; }
    }
}