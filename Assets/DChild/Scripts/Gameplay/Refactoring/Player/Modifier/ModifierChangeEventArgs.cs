using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players
{
    public struct ModifierChangeEventArgs : IEventActionArgs
    {
        public ModifierChangeEventArgs(PlayerModifier modifier, float value) : this()
        {
            this.modifier = modifier;
            this.value = value;
        }

        public PlayerModifier modifier { get; }
        public float value { get; }
    }
}