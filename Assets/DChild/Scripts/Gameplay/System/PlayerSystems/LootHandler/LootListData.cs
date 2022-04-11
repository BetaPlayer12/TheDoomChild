using UnityEngine;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

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

        public void GenerateLootInfo(ref LootList recordList)
        {
            for (int i = 0; i < m_loots.Length; i++)
            {
                m_loots[i].GenerateLootInfo(ref recordList);
            }
        }

#if UNITY_EDITOR
        void ILootDataContainer.DrawDetails(bool drawContainer, string label = null)
        {
            if (drawContainer)
            {
                SirenixEditorGUI.BeginBox(label);
            }

            for (int i = 0; i < m_loots.Length; i++)
            {
                m_loots[i].DrawDetails(false);
            }

            if (drawContainer)
            {
                SirenixEditorGUI.EndBox();
            }
        }
#endif
    }
}