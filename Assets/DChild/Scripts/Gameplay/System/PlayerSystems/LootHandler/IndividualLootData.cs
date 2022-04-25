using UnityEngine;
using Sirenix.OdinInspector;
using DChild.Gameplay.Essence;
using Holysoft.Collections;

#if UNITY_EDITOR
using UnityEditor;
using Sirenix.Utilities.Editor;
#endif

namespace DChild.Gameplay.Systems
{

    [System.Serializable]
    public class IndividualLootData : ILootDataContainer
    {
        [SerializeField]
        private LootReference m_reference;
        [SerializeField, Min(1), OnInspectorGUI("OnLootReferenceGUI")]
        private int m_count = 1;

        public void DropLoot(Vector2 position)
        {
            GameplaySystem.lootHandler.DropLoot(new LootDropRequest(m_reference.loot, m_count, position));
        }

        public void GenerateLootInfo(ref LootList recordList)
        {
            if (m_reference.data == null)
            {
                var soulEssenceValue = m_reference.loot.GetComponent<SoulEssenceLoot>().value;
                soulEssenceValue *= m_count;
                recordList.AddSoulEssence(soulEssenceValue);
            }
            else
            {

                recordList.Add(m_reference.data, m_count);
            }
        }

#if UNITY_EDITOR
        public LootReference reference => m_reference;
        public int count => m_count;

        private void OnLootReferenceGUI()
        {
            var soulEssence = m_reference?.loot?.GetComponent<SoulEssenceLoot>() ?? null;
            if (soulEssence)
            {
                SirenixEditorGUI.InfoMessageBox($"Soul Essence: {soulEssence.value * m_count}");
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
                    EditorGUILayout.LabelField($"{label} ({soulEssence.value * m_count}){suffix}");
                }
                else
                {
                    EditorGUILayout.LabelField($"{label} ({m_count}){suffix}");
                }
            }
        }
#endif
    }
}