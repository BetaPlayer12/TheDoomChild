using UnityEngine;
using Sirenix.OdinInspector;
using DChild.Gameplay.Essence;

#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
using UnityEditor;
#endif

namespace DChild.Gameplay.Systems
{
    [System.Serializable]
    public class RangeIndividualLootData : ILootDataContainer
    {
        [SerializeField]
        private LootReference m_reference;
        [SerializeField, OnInspectorGUI("OnLootReferenceGUI")]
        private Holysoft.Collections.RangeInt m_count;

        public void DropLoot(Vector2 position)
        {
            GameplaySystem.lootHandler.DropLoot(new LootDropRequest(m_reference.loot, m_count.GenerateRandomValue(), position));
        }

        public void GenerateLootInfo(ref LootList recordList)
        {
            recordList.Add(m_reference.data, m_count.GenerateRandomValue());
        }

#if UNITY_EDITOR
        private void OnLootReferenceGUI()
        {
            var soulEssence = m_reference?.loot?.GetComponent<SoulEssenceLoot>() ?? null;
            if (soulEssence)
            {
                SirenixEditorGUI.InfoMessageBox($"Soul Essence: ({soulEssence.value * m_count.min} -  {soulEssence.value * m_count.max})");
            }
        }

        void ILootDataContainer.DrawDetails(bool drawContainer, string label = null)
        {
            if (m_reference != null)
            {
                var soulEssence = m_reference?.loot?.GetComponent<SoulEssenceLoot>() ?? null;
                var suffix = label;
                label = m_reference.name.Replace("LootReference", string.Empty);
                if (soulEssence)
                {
                    EditorGUILayout.LabelField($"{label} ({soulEssence.value * m_count.min} -  {soulEssence.value * m_count.max}){suffix}");
                }
                else
                {
                    EditorGUILayout.LabelField($"{label} ({m_count.min} - {m_count.max}){suffix}");
                }
            }
        }
#endif
    }
}