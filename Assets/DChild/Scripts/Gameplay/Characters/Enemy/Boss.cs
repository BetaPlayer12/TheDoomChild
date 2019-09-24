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

        public event EventAction<PhaseEventArgs> PhaseChange;

        public void SetTarget(IDamageable damageable, Character m_target)
        {
            m_brain.SetTarget(damageable, m_target);
        }

        public void SendPhaseTriggered(int index) => PhaseChange?.Invoke(this, new PhaseEventArgs(index));

        private void Awake()
        {
            m_brain = GetComponent<ICombatAIBrain>();
        }
    }
}
