using DChild.Gameplay.Combat;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Leveling
{
    public class ExperiencePointDropper : MonoBehaviour
    {
        [SerializeField]
        private ExperiencePointDropData m_data;

        private Damageable m_damageable;

        public void GiveEXP()
        {
            GameplaySystem.playerManager.player.level.exp.AddCurrentValue(m_data.exp);
        }

        private void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            GiveEXP();
        }

        private void Awake()
        {
            m_damageable = GetComponentInParent<Damageable>();
            if (m_damageable)
            {
                m_damageable.Destroyed += OnDestroyed;
            }
        }
    }

}