using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using DChild.Menu.Bestiary;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Boss : MonoBehaviour
    {
        public struct PhaseEventArgs : IEventActionArgs
        {
            public PhaseEventArgs(int index) : this()
            {
                this.index = index;
            }

            public int index { get; }
        }

        [SerializeField]
        private BestiaryData m_data;
        [SerializeField]
        private Health m_health;
        private ICombatAIBrain m_brain;

        public Health health => m_health;
        public string creatureName => m_data.creatureName;
        public string creatureTitle => m_data.title;

        public event EventAction<PhaseEventArgs> PhaseChange;

        public void SetTarget(IDamageable damageable, Character m_target)
        {
            m_brain.SetTarget(damageable, m_target);
        }

        public void SendPhaseTriggered(int index) => PhaseChange?.Invoke(this, new PhaseEventArgs(index));

        public void Enable() => m_brain.enabled = true;
        public void Disable() => m_brain.enabled = false;

        private void Awake()
        {
            m_brain = GetComponentInChildren<ICombatAIBrain>(true);
        }
#if UNITY_EDITOR
        public void InitializeFields(BestiaryData data, Health health)
        {
            m_data = data;
            m_health = health;
        }
#endif
    }

}
