using UnityEngine;

namespace DChild.Gameplay.Systems
{
    [CreateAssetMenu(fileName ="LootListData", menuName = "DChild/Gameplay/Loot List Data")]
    public class LootListData : LootData
    {
        [SerializeField]
        private LootData[] m_loots;

        public override void DropLoot(Vector2 position)
        {
            for (int i = 0; i < m_loots.Length; i++)
            {
                m_loots[i].DropLoot(position);
            }
        }
    }
}