using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Attributes
{
    public abstract class Attribute :  IValueChange
    {
        public event EventAction<EventActionArgs> ValueChanged;

        [SerializeField, OnValueChanged("ValueChange"), LabelText("$attributeName"), Min(1)]
        protected int m_value = 1;
        public int value => m_value;

        public void SetValue(int value)
        {
            m_value = value;
            CalculateBonuses();
            ValueChanged?.Invoke(this, EventActionArgs.Empty);
        }

        public abstract void CalculateBonuses();

#if UNITY_EDITOR
        protected string attributeName => GetType().Name;

        private void ValueChange()
        {
            if(m_value < 0)
            {
                m_value = 0;
            }
            CalculateBonuses();
        }
#endif
    }
}