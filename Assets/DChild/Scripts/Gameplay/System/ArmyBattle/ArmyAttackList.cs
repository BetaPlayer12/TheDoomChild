using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class ArmyAttackList
    {
        [SerializeField, TabGroup("Attack Group/Tab", "Rock"), ListDrawerSettings(HideAddButton = true), PropertyOrder(4)]
        private List<ArmyAttackGroup> m_rockAttackers;
        [SerializeField, TabGroup("Attack Group/Tab", "Paper"), ListDrawerSettings(HideAddButton = true), PropertyOrder(4)]
        private List<ArmyAttackGroup> m_paperAttackers;
        [SerializeField, TabGroup("Attack Group/Tab", "Scissors"), ListDrawerSettings(HideAddButton = true), PropertyOrder(4)]
        private List<ArmyAttackGroup> m_scissorAttackers;

        public ArmyAttackList()
        {
            m_rockAttackers = new List<ArmyAttackGroup>();
            m_paperAttackers = new List<ArmyAttackGroup>();
            m_scissorAttackers = new List<ArmyAttackGroup>();
        }

        public ArmyAttackList(params ArmyAttackGroup[] armyCharacter):this()
        {
            SetAttackGroups(armyCharacter);
        }

        public ArmyAttackList(ArmyAttackList reference) : this()
        {
            GenerateCopiesOfArmyAttackGroup(reference);
        }

        public void AddAttackGroup(ArmyAttackGroup armyAttackGroup)
        {
            //var list = GetAttackGroupsOfUnityType(armyAttackGroup.unitType);
            //if (list.Contains(armyAttackGroup) == false)
            //{
            //    list.Add(armyAttackGroup);
            //    //Find A Way to Modify Same Group with different Member Count
            //}
        }

        public void RemoveAttackGroup(ArmyAttackGroup armyAttackGroup)
        {
            //var list = GetAttackGroupsOfUnityType(armyAttackGroup.unitType);
            //list.Remove(armyAttackGroup);
        }

        public ArmyAttackGroup RemoveAttackGroup(UnitType unitType, int index)
        {
            if (index < 0)
                return null;

            var characters = GetAttackGroupsOfUnityType(unitType);

            if (index >= characters.Count)
                return null;

            var toRemove = characters[index];
            characters.RemoveAt(index);
            return toRemove;
        }

        public void SetAttackGroups(params ArmyAttackGroup[] armyCharacters)
        {
            ClearAttackGroups();
            for (int i = 0; i < armyCharacters.Length; i++)
            {
                AddAttackGroup(armyCharacters[i]);
            }
        }

        public void ClearAttackGroups()
        {
            m_rockAttackers.Clear();
            m_paperAttackers.Clear();
            m_scissorAttackers.Clear();
        }

        public List<ArmyAttackGroup> GetAttackGroupsOfUnityType(UnitType unitType)
        {
            switch (unitType)
            {
                case UnitType.Rock:
                    return m_rockAttackers;
                case UnitType.Paper:
                    return m_paperAttackers;
                case UnitType.Scissors:
                    return m_scissorAttackers;
                default:
                    return null;
            }
        }

        public void ResetAvailability()
        {
            ResetAvailability(GetAttackGroupsOfUnityType(UnitType.Rock));
            ResetAvailability(GetAttackGroupsOfUnityType(UnitType.Paper));
            ResetAvailability(GetAttackGroupsOfUnityType(UnitType.Scissors));

            void ResetAvailability(List<ArmyAttackGroup> groups)
            {
                for (int i = 0; i < groups.Count; i++)
                {
                    //groups[i].SetAvailability(true);
                }
            }
        }

        private void GenerateCopiesOfArmyAttackGroup(ArmyAttackList reference)
        {
            GenerateCopyOf(reference.GetAttackGroupsOfUnityType(UnitType.Rock), GetAttackGroupsOfUnityType(UnitType.Rock));
            GenerateCopyOf(reference.GetAttackGroupsOfUnityType(UnitType.Paper), GetAttackGroupsOfUnityType(UnitType.Paper));
            GenerateCopyOf(reference.GetAttackGroupsOfUnityType(UnitType.Scissors), GetAttackGroupsOfUnityType(UnitType.Scissors));
            void GenerateCopyOf(List<ArmyAttackGroup> reference, List<ArmyAttackGroup> destination)
            {
                destination.Clear();
                for (int i = 0; i < reference.Count; i++)
                {
                    //destination.Add(new ArmyAttackGroup(reference[i].reference));
                }
            }
        }
    }
}