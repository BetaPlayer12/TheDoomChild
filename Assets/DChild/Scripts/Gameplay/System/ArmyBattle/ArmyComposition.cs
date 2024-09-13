using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class ArmyComposition
    {
        [SerializeField]
        private string m_name;

        [SerializeField, MinValue(1)]
        private int m_troopCount = 1;

        [SerializeField]
        private ArmyAttackList m_attacks;
        [SerializeField]
        private ArmyAbilityList m_abilities;

        public string name => m_name;
        public int troopCount => m_troopCount;
        public ArmyAttackList attacks => m_attacks;
        public ArmyAbilityList abilities => m_abilities;

        public ArmyComposition()
        {
            m_name = "Battalion";
            m_troopCount = 1;
            m_attacks = new ArmyAttackList();
            m_abilities = new ArmyAbilityList();
        }

        public ArmyComposition(string name, int troopCount, ArmyAttackGroup[] attackers, ArmyAbilityGroup[] abilities)
        {
            m_name = name;
            m_troopCount = troopCount;
            m_attacks = new ArmyAttackList(attackers);
            m_abilities = new ArmyAbilityList(abilities);
        }

        public ArmyComposition(ArmyComposition reference)
        {
            m_name = reference.name;
            m_troopCount = reference.troopCount;
            m_attacks = new ArmyAttackList(reference.attacks);
            m_abilities = new ArmyAbilityList(reference.abilities);
        }
        //#if UNITY_EDITOR
        //        [SerializeField, AssetSelector, FoldoutGroup("Attack Group/Editor"), PropertyOrder(2)]
        //        private ArmyAttackGroupData[] m_attackGroupToAddList;


        //        [Button, ButtonGroup("Attack Group/Editor/Options"), PropertyOrder(3), ShowIf("@m_attackGroupToAddList.Length > 0")]
        //        private void AddCharactersToComposition()
        //        {
        //            for (int i = 0; i < m_attackGroupToAddList.Length; i++)
        //            {
        //                AddAttackGroup(new ArmyAttackGroup(m_attackGroupToAddList[i]));
        //            }
        //            m_attackGroupToAddList = new ArmyAttackGroupData[0];
        //        }

        //        [Button, ButtonGroup("Attack Group/Editor/Options"), PropertyOrder(3), ShowIf("@m_attackGroupToAddList.Length > 0")]
        //        private void SetCharactersToComposition()
        //        {
        //            var newGroups = new ArmyAttackGroup[m_attackGroupToAddList.Length];

        //            for (int i = 0; i < newGroups.Length; i++)
        //            {
        //                newGroups[i] = new ArmyAttackGroup(m_attackGroupToAddList[i]);
        //            }
        //            SetAttackGroups(newGroups);
        //            m_attackGroupToAddList = new ArmyAttackGroupData[0];
        //        }
        //#endif
    }
}