using DChild.Gameplay.Systems;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay.Systems
{
    [CustomEditor(typeof(LootDropData))]
    public class LootDropData_Inspector : OdinEditor
    {
        public class Info
        {
            public int count;
            public List<float> chances;

            public Info()
            {
                count = 0;
                chances = new List<float>();
            }

            public void Set(LootDropData data)
            {
                var drops = data.drops;
                count = drops.Count;
                chances.Clear();
                for (int i = 0; i < count; i++)
                {
                    chances.Add(drops[i].chance);
                }
            }
        }

        private Info m_previousInfo;

        public override void OnInspectorGUI()
        {
            var lootDropData = target as LootDropData;
            m_previousInfo.Set(lootDropData);
            base.OnInspectorGUI();
            if (lootDropData.m_autoCalculate)
            {
                if (lootDropData.m_forcedEdit == true)
                {
                    lootDropData.m_forcedEdit = false;
                }
                else
                {
                    var drops = lootDropData.drops;
                    for (int i = 0; i < m_previousInfo.count; i++)
                    {
                        if (m_previousInfo.chances[i] != drops[i].chance)
                        {
                            List<LootDropData.DropInfo> viableList = new List<LootDropData.DropInfo>();
                            viableList.AddRange(drops);
                            viableList.RemoveAt(i);
                            var difference = drops[i].chance - m_previousInfo.chances[i];
                            if (difference > 0)
                            {
                                LootDropData.DeductChanceFromList(viableList, difference);
                            }
                            else
                            {
                                LootDropData.AddChanceFromList(viableList, -difference);
                            }
                            LootDropData.CalculatePercentScore(lootDropData.drops);
                            break;
                        }
                    }
                } 
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            m_previousInfo = new Info();
        }
    }
}