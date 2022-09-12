using DChild.Inputs;
using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public struct ControllerEventArgs : IEventActionArgs
    {
        public ControllerEventArgs(PlayerInputOld input)
        {
            this.input = input;
        }

        public PlayerInputOld input { get; }
    }
}