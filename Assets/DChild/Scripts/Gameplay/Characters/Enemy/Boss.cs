using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using DChild.Menu.Bestiary;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Boss : MonoBehaviour
    {
        [SerializeField]
        private BestiaryData m_data;
        [SerializeField]
        private Health m_health;
        private ICombatAIBrain m_brain;

        public Health health => m_health;

        public void SetTarget(IDamageable damageable, Character m_target)
        {
            m_brain.SetTarget(damageable, m_target);
        }

        private void Awake()
        {
            m_brain = GetComponent<ICombatAIBrain>();
        }
    }
}
