using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.State
{
    public struct EnrageEventArgs : IEventActionArgs
    {
        public EnrageEventArgs(bool value) : this()
        {
            this.value = value;
        }

        public bool value { get; }
    }

    public interface IEnragedState
    {
        bool isEnraged { get; set; }

        event EventAction<EnrageEventArgs> EnrageChange;
    }
}
