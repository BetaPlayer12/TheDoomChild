using UnityEngine;

namespace DChild.Gameplay.Systems
{
    [System.Serializable]
    public class LootListData : ILootDataContainer
    {
        [SerializeField]
        private ILootDataContainer[] m_loots = new ILootDataContainer[1];

        public void DropLoot(Vector2 position)
        {
            for (int i = 0; i < m_loots.Length; i++)
            {
                m_loots[i].DropLoot(position);
            }
        }
    }
}