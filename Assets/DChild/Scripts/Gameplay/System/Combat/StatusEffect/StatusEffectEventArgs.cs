using Holysoft.Event;

namespace DChild.Gameplay.Combat.StatusInfliction
{
    public struct StatusEffectEventArgs : IEventActionArgs
    {
        public StatusEffectEventArgs(IStatusReciever reciever, StatusEffect effect) : this()
        {
            this.reciever = reciever;
            this.effect = effect;
            this.type = effect.type;
        }

        public IStatusReciever reciever { get; private set; }
        public StatusEffect effect { get; private set; }
        public StatusEffectType type { get; private set; }

        public void Set(IStatusReciever reciever, StatusEffect effect, StatusEffectType type)
        {
            this.reciever = reciever;
            this.effect = effect;
            this.type = type;
        }
    }
}