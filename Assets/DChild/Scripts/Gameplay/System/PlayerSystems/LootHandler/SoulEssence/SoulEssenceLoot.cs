
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Essence
{
    public class SoulEssenceLoot : EssenceLoot
    {
        [SerializeField, Min(1)]
        private int m_value;

        public int value => m_value;
#if UNITY_EDITOR

        [Button]
        private void StartPop()
        {
            SpawnAt(transform.position, transform.rotation);
        }
#endif

        protected override void OnApplyPickup(IPlayer player)
        {
            player.inventory.AddSoulEssence(m_value);
        }
    }
}