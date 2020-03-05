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
        private IBossPhaseInfo m_bossPhaseInfo;
        private int[] m_healthPercentagePhaseInfo;

        public Health health => m_health;
        public int[] healthSegments { get; }
        public string creatureName => m_data.name;
        public string creatureTitle => m_data.title;

        public event EventAction<PhaseEventArgs> PhaseChange;

        public void SetTarget(IDamageable damageable, Character m_target)
        {
            m_brain.SetTarget(damageable, m_target);
        }

        public void SendPhaseTriggered(int index) => PhaseChange?.Invoke(this, new PhaseEventArgs(index));

        private void Awake()
        {
            m_brain = GetComponent<ICombatAIBrain>();
            m_bossPhaseInfo = GetComponent<IBossPhaseInfo>();
        }

        private void Start()
        {
            m_healthPercentagePhaseInfo = m_bossPhaseInfo.GetHealthPrecentagePhaseInfo();
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
