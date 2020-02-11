using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Essence
{
    public class HealthEssenceLoot : EssenceLoot
    {
        [SerializeField, Min(1)]
        private int m_healValue;

        protected override void OnApplyPickup(IPlayer player)
        {
            GameplaySystem.combatManager.Heal((IHealable)player.damageableModule, m_healValue);
        }
    }
}