using DChild.Gameplay.Characters.Players.Attributes;
using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players
{
    public interface IAttributes
    {
        event EventAction<AttributeValueEventArgs> ValueChange;
        int GetValue(PlayerAttribute attribute);
        void SetValue(PlayerAttribute attribute, int value);
        void AddValue(PlayerAttribute attribute, int value);
    }

    public struct AttributeValueEventArgs : IEventActionArgs
    {
        public AttributeValueEventArgs(PlayerAttribute atttribute, int value) : this()
        {
            this.atttribute = atttribute;
            this.value = value;
        }

        public PlayerAttribute atttribute { get; }
        public int value { get; }
    }
}