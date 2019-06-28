using DChild.Gameplay.Characters.Players.Attributes;
using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players
{
    public interface IAttributes
    {
        event EventAction<AttributeValueEventArgs> ValueChange;
        int GetValue(Attribute attribute);
        void SetValue(Attribute attribute, int value);
        void AddValue(Attribute attribute, int value);
    }

    public struct AttributeValueEventArgs : IEventActionArgs
    {
        public AttributeValueEventArgs(Attribute atttribute, int value) : this()
        {
            this.atttribute = atttribute;
            this.value = value;
        }

        public Attribute atttribute { get; }
        public int value { get; }
    }
}