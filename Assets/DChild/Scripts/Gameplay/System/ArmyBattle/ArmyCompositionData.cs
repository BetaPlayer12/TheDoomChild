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
        [DetailedInfoBox("@GetSummarryAbilityMessage()", "@GetDetailedAbilityMessage()", InfoMessageType = InfoMessageType.None)]
#endif
        [SerializeField, AssetSelector, PropertyOrder(2), InlineEditor(InlineEditorObjectFieldModes.Foldout, Expanded = true)]
        private ArmyGroupTemplateData[] m_groups;

        public string name => m_name;
        public int troopCount => m_troopCount;

        public ArmyComposition GenerateArmyCompositionInstance()
        {
            List<ArmyAttackGroup> attackgroups = new List<ArmyAttackGroup>();
            List<ArmyAbilityGroup> abilitygroups = new List<ArmyAbilityGroup>();
            for (int i = 0; i < m_groups.Length; i++)
            {
                var group = m_groups[i];
                //if (group.canAttack)
                //{
                //    attackgroups.Add(new ArmyAttackGroup(group));
                //}
                //if (group.hasAbility)
                //{
                //    abilitygroups.Add(new ArmyAbilityGroup(group));
                //}
            }
            return new ArmyComposition(m_name, m_troopCount, attackgroups.ToArray(), abilitygroups.ToArray());
        }

#if UNITY_EDITOR

        private string GetSummarryAbilityMessage()
        {
            return $"Ability Groups \n" +
                 $"Count: {GetTotalAbilities()}";
        }

        private int GetTotalAbilities()
        {
            int abilities = 0;
            for (int i = 0; i < m_groups.Length; i++)
            {
                var group = m_groups[i];
                if (group.hasAbility)
                {
                    abilities++;
                }
            }

            return abilities;
        }

        private string GetDetailedAbilityMessage()
        {
            var message = $"Ability Groups";
            for (int i = 0; i < m_groups.Length; i++)
            {
                var group = m_groups[i];
                if (group.hasAbility)
                {
                    //message += "\n" + GetDetail(group);
                }
            }
            return message;
            //string GetDetail(ISpecialSkillGroup data) => $"{data.}({data.abilityDescription})";
        }

        private string GetSummarryMessage(UnitType unitType)
        {
            return $"{unitType.ToString()} Groups \n" +
                $"Count: {GetTotalCount(unitType)} \n" +
                $"Total Power: {GetTotalPower(unitType)}";
        }

        private int GetTotalPower(UnitType type)
        {
            int power = 0;
            for (int i = 0; i < m_groups.Length; i++)
            {
                var attackGroup = m_groups[i];
                if (attackGroup.attackType == type)
                {
                    power += attackGroup.GetTotalAttackPower();
                }
            }

            return power;
        }

        private int GetTotalCount(UnitType type)
        {
            int count = 0;
            for (int i = 0; i < m_groups.Length; i++)
            {
                var attackGroup = m_groups[i];
                if (attackGroup.attackType == type)
                {
                    count++;
                }
            }

            return count;
        }

        private string GetDetailedMessage(UnitType unitType)
        {
            var message = $"{unitType.ToString()} Groups";
            for (int i = 0; i < m_groups.Length; i++)
            {
                var attackGroup = m_groups[i];
                if (attackGroup.attackType == unitType)
                {
                    //message += "\n" + GetDetail(attackGroup);
                }
            }
            return message;
            //string GetDetail(IAttackGroup data) => $"{data.groupName}({data.GetAttackPower()})";
        }
#endif

    }
}