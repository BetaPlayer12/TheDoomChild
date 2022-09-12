
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Essence
{
    public class AetherLoot : EssenceLoot
    {
        [SerializeField, Min(1)]
        private int m_value;

#if UNITY_EDITOR
        public int value => m_value;

        [Button]
        private void StartPop()
        {
            SpawnAt(transform.position, transform.rotation);
        }
#endif

        protected override void OnApplyPickup(IPlayer player)
        {
            //Add Soul SKill Capacity Point
        }
    }
}