using DChild.Gameplay.Combat;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Player.CombatArt.Leveling
{
    public class CombatArtExperienceDropper : MonoBehaviour
    {
        [SerializeField]
        private CombatArtExperienceDropData m_data;

        private Damageable m_damageable;

        public void GiveEXP()
        {
            GameplaySystem.playerManager.player.combatArts.level.exp.AddCurrentValue(m_data.exp);
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