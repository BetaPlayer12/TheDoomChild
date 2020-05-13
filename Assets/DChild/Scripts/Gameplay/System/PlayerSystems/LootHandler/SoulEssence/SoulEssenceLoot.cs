using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Essence
{
    public class SoulEssenceLoot : EssenceLoot
    {
        [SerializeField, Min(1)]
        private int m_value;

#if UNITY_EDITOR
        public int value => m_value;
#endif

        protected override void OnApplyPickup(IPlayer player)
        {
            player.inventory.AddSoulEssence(m_value);
        }
    }
}