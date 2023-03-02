using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [CreateAssetMenu(fileName = "ArmyCompositionData", menuName = "DChild/Gameplay/Army/Army Composition Data")]
    public class ArmyCompositionData : ScriptableObject
    {
        [SerializeField]
        private string m_name;

        [SerializeField, MinValue(1)]
        private int m_troopCount = 1;

#if UNITY_EDITOR
        [DetailedInfoBox("@GetSummarryMessage(UnitType.Rock)", "@GetDetailedMessage(UnitType.Rock)", InfoMessageType = InfoMessageType.None)]
        [DetailedInfoBox("@GetSummarryMessage(UnitType.Paper)", "@GetDetailedMessage(UnitType.Paper)", InfoMessageType = InfoMessageType.None)]
        [DetailedInfoBox("@GetSummarryMessage(UnitType.Scissors)", "@GetDetailedMessage(UnitType.Scissors)", InfoMessageType = InfoMessageType.None)]
#endif
        [SerializeField, AssetSelector, PropertyOrder(2), InlineEditor(InlineEditorObjectFieldModes.Foldout, Expanded = true)]
        private ArmyAttackGroupData[] m_attackGroups;

        public string name => m_name;
        public int troopCount => m_troopCount;

        public ArmyComposition GenerateArmyCompositionInstance()
        {
            var armyAttackGroup = new ArmyAttackGroup[m_attackGroups.Length];
            for (int i = 0; i < m_attackGroups.Length; i++)
            {
                armyAttackGroup[i] = new ArmyAttackGroup(m_attackGroups[i]);
            }
            return new ArmyComposition(m_name, m_troopCount, armyAttackGroup);
        }

#if UNITY_EDITOR

        private string GetSummarryMessage(UnitType unitType)
        {
            return $"{unitType.ToString()} Groups \n" +
                $"Count: {GetTotalCount(unitType)} \n" +
                $"Total Power: {GetTotalPower(unitType)}";
        }

        private int GetTotalPower(UnitType type)
        {
            int power = 0;
            for (int i = 0; i < m_attackGroups.Length; i++)
            {
                var attackGroup = m_attackGroups[i];
                if (attackGroup.unitType == type)
                {
                    power += attackGroup.GetTotalPower();
                }
            }

            return power;
        }

        private int GetTotalCount(UnitType type)
        {
            int count = 0;
            for (int i = 0; i < m_attackGroups.Length; i++)
            {
                var attackGroup = m_attackGroups[i];
                if (attackGroup.unitType == type)
                {
                    count++;
                }
            }

            return count;
        }

        private string GetDetailedMessage(UnitType unitType)
        {
            var message = $"{unitType.ToString()} Groups";
            for (int i = 0; i < m_attackGroups.Length; i++)
            {
                var attackGroup = m_attackGroups[i];
                if (attackGroup.unitType == unitType)
                {
                    message += "\n" + GetDetail(attackGroup);
                }
            }
            return message;
            string GetDetail(ArmyAttackGroupData data) => $"{data.groupName}({data.GetTotalPower()})";
        }
#endif

    }
}