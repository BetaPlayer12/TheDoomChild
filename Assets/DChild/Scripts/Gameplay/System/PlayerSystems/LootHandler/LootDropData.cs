using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
using Sirenix.Utilities.Editor;
#endif

namespace DChild.Gameplay.Systems
{
    [System.Serializable]
    public class LootDropData : ILootDataContainer
    {
        [System.Serializable]
        public class DropInfo
        {
            [SerializeField]
            private ILootDataContainer m_loot;
            [SerializeField, Range(0, 100)]
            private float m_chance;
            [SerializeField, ReadOnly]
            private float m_percentScore;

            public ILootDataContainer loot { get => m_loot; }
            public float chance
            {
                get => m_chance;
#if UNITY_EDITOR
                set
                {
                    m_chance = value;
                }
#endif
            }

            public float percentScore
            {
                get => m_percentScore;
#if UNITY_EDITOR
                set
                {
                    m_percentScore = value;
                }
#endif 
            }
        }

        [SerializeField, ListDrawerSettings(CustomAddFunction = "AddElement", CustomRemoveElementFunction = "RemoveElement", DraggableItems = false, OnTitleBarGUI = "CreateToolbar")]
        private List<DropInfo> m_drops = new List<DropInfo>();

        public List<DropInfo> drops { get => m_drops; set => m_drops = value; }

        private ILootDataContainer GetRandomLoot()
        {
            if (m_drops.Count > 0)
            {
                var score = UnityEngine.Random.Range(0f, 100f);
                int index = 0;
                while (index < m_drops.Count && index != m_drops.Count - 1 && m_drops[index].percentScore > score)
                {
                    index++;
                }
                return m_drops[index].loot;
            }
            else
            {
                return null;
            }
        }

        public void DropLoot(Vector2 position)
        {
            GetRandomLoot()?.DropLoot(position);
        }

        public void GenerateLootInfo(ref LootList recordList)
        {
            GetRandomLoot()?.GenerateLootInfo(ref recordList);
        }

#if UNITY_EDITOR
        [NonSerialized]
        public bool m_forcedEdit;
        [NonSerialized, ShowInInspector, PropertyOrder(-1)]
        public bool m_autoCalculate;

        void ILootDataContainer.DrawDetails(bool drawContainer, string label = null)
        {
            SirenixEditorGUI.BeginBox(label);
            EditorGUI.indentLevel++;
            for (int i = 0; i < m_drops.Count; i++)
            {
                if (m_drops[i].loot == null)
                {
                    EditorGUILayout.LabelField($"None - {m_drops[i].chance}%");
                }
                else
                {
                    m_drops[i].loot.DrawDetails(true, $" - {m_drops[i].chance}%");
                }
            }
            EditorGUI.indentLevel--;
            SirenixEditorGUI.EndBox();
        }

        private void CreateToolbar()
        {
            if (m_drops.Count >= 2)
            {
                if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
                {
                    EquilizeChance();
                    CalculatePercentScore(m_drops);
                }
            }
        }

        private void AddElement()
        {
            var newCount = m_drops.Count + 1;
            var chance = 100f / newCount;
            var dropInfo = new DropInfo();
            dropInfo.chance = chance;
            if (m_drops.Count > 0)
            {
                DeductChanceFromList(m_drops, chance);
            }
            m_drops.Add(dropInfo);
            CalculatePercentScore(m_drops);
            m_forcedEdit = true;
        }

        private void RemoveElement(DropInfo info)
        {
            m_drops.Remove(info);
            if (m_drops.Count > 0)
            {
                AddChanceFromList(m_drops, info.chance);
                CalculatePercentScore(m_drops);
            }
            m_forcedEdit = true;
        }

        private void EquilizeChance()
        {
            var chance = 100f / m_drops.Count;
            for (int i = 0; i < m_drops.Count; i++)
            {
                m_drops[i].chance = chance;
            }
            m_forcedEdit = true;
        }

        public static void CalculatePercentScore(List<DropInfo> list)
        {
            float score = 0;
            for (int i = 0; i < list.Count - 1; i++)
            {
                score += list[i].chance;
                list[i].percentScore = score;
            }
            list[list.Count - 1].percentScore = 100;
        }

        public static void DeductChanceFromList(List<DropInfo> list, float deduction)
        {
            List<DropInfo> viableList = new List<DropInfo>();
            viableList.AddRange(list);
            for (int i = viableList.Count - 1; i >= 0; i--)
            {
                if (viableList[i].chance == 0)
                {
                    viableList.RemoveAt(i);
                }
            }

            if (viableList.Count > 0)
            {
                var decriment = deduction / viableList.Count;
                do
                {
                    var lowestChanceFromDrop = viableList.Select(x => x.chance).Min();
                    if (decriment < lowestChanceFromDrop)
                    {
                        for (int i = 0; i < viableList.Count; i++)
                        {
                            viableList[i].chance -= decriment;
                        }
                        deduction = 0;
                    }
                    else
                    {
                        deduction -= lowestChanceFromDrop * viableList.Count;

                        for (int i = viableList.Count - 1; i >= 0; i--)
                        {
                            viableList[i].chance -= lowestChanceFromDrop;
                            if (viableList[i].chance == 0)
                            {
                                viableList.RemoveAt(i);
                            }
                        }
                    }
                } while (deduction > 0);

            }
        }

        public static void AddChanceFromList(List<DropInfo> list, float value)
        {
            List<DropInfo> viableList = new List<DropInfo>();
            viableList.AddRange(list);
            do
            {
                var increment = value / viableList.Count;
                for (int i = 0; i < viableList.Count; i++)
                {
                    viableList[i].chance += increment;
                }
                value = 0;
                for (int i = viableList.Count - 1; i >= 0; i--)
                {
                    var chance = viableList[i].chance;
                    if (chance > 100)
                    {
                        value += chance - 100;
                        viableList.RemoveAt(i);
                    }
                }
            }
            while (value > 0 && viableList.Count > 0);
        }
#endif
    }
}