using DChildEditor;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{

    [System.Serializable]
    public class ArmyComposition
    {
        [System.Serializable]
        public class SaveData
        {
            private int[] m_rockCharacterIDs;
            private int[] m_paperCharacterIDs;
            private int[] m_scissorsCharacterIDs;

            public SaveData(IReadOnlyCollection<ArmyCharacter> rockCharacters, IReadOnlyCollection<ArmyCharacter> paperCharacters, IReadOnlyCollection<ArmyCharacter> scissorsCharacters)
            {
                m_rockCharacterIDs = CreateCharacterIDList(rockCharacters);
                m_paperCharacterIDs = CreateCharacterIDList(paperCharacters);
                m_scissorsCharacterIDs = CreateCharacterIDList(scissorsCharacters);
            }

            public int GetTotalCharacters(UnitType unitType)
            {
                switch (unitType)
                {
                    case UnitType.Rock:
                        return m_rockCharacterIDs.Length;
                    case UnitType.Paper:
                        return m_paperCharacterIDs.Length;
                    case UnitType.Scissors:
                        return m_scissorsCharacterIDs.Length;
                    default:
                        return 0;
                }
            }

            public int GetCharacterID(UnitType unitType, int index)
            {
                switch (unitType)
                {
                    case UnitType.Rock:
                        return m_rockCharacterIDs[index];
                    case UnitType.Paper:
                        return m_paperCharacterIDs[index];
                    case UnitType.Scissors:
                        return m_scissorsCharacterIDs[index];
                    default:
                        return -1;
                }
            }

            private int[] CreateCharacterIDList(IReadOnlyCollection<ArmyCharacter> characters)
            {
                var rockCharacterCount = characters.Count;
                var characterIDs = new int[rockCharacterCount];
                for (int i = 0; i < rockCharacterCount; i++)
                {
                    m_rockCharacterIDs[i] = characters.ElementAt(i).ID;
                }

                return characterIDs;
            }
        }

        [SerializeField]
        private string m_name;

        [SerializeField, MinValue(1)]
        private int m_troopCount = 1;
        [FoldoutGroup("Attack Group")]

        [SerializeField, TabGroup("Attack Group/Tab", "Rock"), ListDrawerSettings(HideAddButton = true), PropertyOrder(4), InlineEditor(InlineEditorObjectFieldModes.Foldout, Expanded = true)]
        private List<ArmyAttackGroup> m_rockAttackers;
        [SerializeField, TabGroup("Attack Group/Tab", "Paper"), ListDrawerSettings(HideAddButton = true), PropertyOrder(4), InlineEditor(InlineEditorObjectFieldModes.Foldout, Expanded = true)]
        private List<ArmyAttackGroup> m_paperAttackers;
        [SerializeField, TabGroup("Attack Group/Tab", "Scissors"), ListDrawerSettings(HideAddButton = true), PropertyOrder(4), InlineEditor(InlineEditorObjectFieldModes.Foldout, Expanded = true)]
        private List<ArmyAttackGroup> m_scissorAttackers;

        public string name => m_name;
        public int troopCount => m_troopCount;

        public ArmyComposition()
        {
            m_name = "Battalion";
            m_troopCount = 1;
            m_rockAttackers = new List<ArmyAttackGroup>();
            m_paperAttackers = new List<ArmyAttackGroup>();
            m_scissorAttackers = new List<ArmyAttackGroup>();
        }

        public ArmyComposition(string name, int troopCount, params ArmyAttackGroup[] armyCharacters)
        {
            m_name = name;
            m_troopCount = troopCount;
            m_rockAttackers = new List<ArmyAttackGroup>();
            m_paperAttackers = new List<ArmyAttackGroup>();
            m_scissorAttackers = new List<ArmyAttackGroup>();
            SetAttackGroups(armyCharacters);
        }

        public ArmyComposition(ArmyComposition reference)
        {
            m_name = reference.name;
            m_troopCount = reference.troopCount;
            m_rockAttackers = new List<ArmyAttackGroup>(reference.GetAttackGroupsOfUnityType(UnitType.Rock));
            m_paperAttackers = new List<ArmyAttackGroup>(reference.GetAttackGroupsOfUnityType(UnitType.Paper));
            m_scissorAttackers = new List<ArmyAttackGroup>(reference.GetAttackGroupsOfUnityType(UnitType.Scissors));
        }


        public void CopyComposition(ArmyComposition reference)
        {
            m_name = reference.name;
            m_troopCount = reference.troopCount;
            m_rockAttackers.Clear();
            m_rockAttackers.AddRange(reference.GetAttackGroupsOfUnityType(UnitType.Rock));
            m_paperAttackers.Clear();
            m_paperAttackers.AddRange(reference.GetAttackGroupsOfUnityType(UnitType.Paper));
            m_scissorAttackers.Clear();
            m_scissorAttackers.AddRange(reference.GetAttackGroupsOfUnityType(UnitType.Scissors));
        }

        public void AddAttackGroup(ArmyAttackGroup armyAttackGroup)
        {
            var list = GetAttackGroupsOfUnityType(armyAttackGroup.unitType);
            if (list.Contains(armyAttackGroup) == false)
            {
                list.Add(armyAttackGroup);
                //Find A Way to Modify Same Group with different Member Count
            }
        }

        public void RemoveAttackGroup(ArmyAttackGroup armyAttackGroup)
        {
            var list = GetAttackGroupsOfUnityType(armyAttackGroup.unitType);
            list.Remove(armyAttackGroup);
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
            ClearCharacters();
            for (int i = 0; i < armyCharacters.Length; i++)
            {
                AddAttackGroup(armyCharacters[i]);
            }
        }

        public void ClearCharacters()
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

#if UNITY_EDITOR
        [SerializeField, AssetSelector, FoldoutGroup("Attack Group/Editor"), PropertyOrder(2)]
        private ArmyAttackGroupData[] m_attackGroupToAddList;


        [Button, ButtonGroup("Attack Group/Editor/Options"), PropertyOrder(3), ShowIf("@m_attackGroupToAddList.Length > 0")]
        private void AddCharactersToComposition()
        {
            for (int i = 0; i < m_attackGroupToAddList.Length; i++)
            {
                AddAttackGroup(new ArmyAttackGroup(m_attackGroupToAddList[i]));
            }
            m_attackGroupToAddList = new ArmyAttackGroupData[0];
        }

        [Button, ButtonGroup("Attack Group/Editor/Options"), PropertyOrder(3), ShowIf("@m_attackGroupToAddList.Length > 0")]
        private void SetCharactersToComposition()
        {
            var newGroups = new ArmyAttackGroup[m_attackGroupToAddList.Length];

            for (int i = 0; i < newGroups.Length; i++)
            {
                newGroups[i] = new ArmyAttackGroup(m_attackGroupToAddList[i]);
            }
            SetAttackGroups(newGroups);
            m_attackGroupToAddList = new ArmyAttackGroupData[0];
        }
#endif
    }
}