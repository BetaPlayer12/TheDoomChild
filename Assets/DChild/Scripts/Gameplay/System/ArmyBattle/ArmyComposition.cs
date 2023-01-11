using DChildEditor;
using Sirenix.OdinInspector;
using System.Collections.Generic;
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
        [FoldoutGroup("Characters")]

        [SerializeField, TabGroup("Characters/Tab", "Rock"), ListDrawerSettings(HideAddButton = true), PropertyOrder(4), InlineEditor(InlineEditorObjectFieldModes.Foldout, Expanded = true)]
        private List<ArmyCharacter> m_rockCharacters;
        [SerializeField, TabGroup("Characters/Tab", "Paper"), ListDrawerSettings(HideAddButton = true), PropertyOrder(4), InlineEditor(InlineEditorObjectFieldModes.Foldout, Expanded = true)]
        private List<ArmyCharacter> m_paperCharacters;
        [SerializeField, TabGroup("Characters/Tab", "Scissors"), ListDrawerSettings(HideAddButton = true), PropertyOrder(4), InlineEditor(InlineEditorObjectFieldModes.Foldout, Expanded = true)]
        private List<ArmyCharacter> m_scissorCharacters;

        public int troopCount => m_troopCount;

        public ArmyComposition()
        {
            m_name = "Battalion";
            m_troopCount = 1;
            m_rockCharacters = new List<ArmyCharacter>();
            m_paperCharacters = new List<ArmyCharacter>();
            m_scissorCharacters = new List<ArmyCharacter>();
        }

        public ArmyComposition(string name, int troopCount, params ArmyCharacter[] armyCharacters)
        {
            m_name = name;
            m_troopCount = troopCount;
            m_rockCharacters = new List<ArmyCharacter>();
            m_paperCharacters = new List<ArmyCharacter>();
            m_scissorCharacters = new List<ArmyCharacter>();
            SetCharacters(armyCharacters);
        }

        public ArmyComposition(ArmyComposition reference)
        {
            m_name = reference.name;
            m_troopCount = reference.troopCount;
            m_rockCharacters = new List<ArmyCharacter>(reference.GetCharactersOfUnityType(UnitType.Rock));
            m_paperCharacters = new List<ArmyCharacter>(reference.GetCharactersOfUnityType(UnitType.Paper));
            m_scissorCharacters = new List<ArmyCharacter>(reference.GetCharactersOfUnityType(UnitType.Scissors));
        }

        public string name => m_name;

        public void AddCharacter(ArmyCharacter character)
        {
            GetCharactersOfUnityType(character.unitType).Add(character);
        }

        public void RemoveCharacter(ArmyCharacter character)
        {
            GetCharactersOfUnityType(character.unitType).Remove(character);
        }

        public void RemoveCharacter(UnitType unitType, int index)
        {
            if (index < 0)
                return;

            var characters = GetCharactersOfUnityType(unitType);

            if (index >= characters.Count)
                return;

            characters.RemoveAt(index);
        }

        public void SetCharacters(params ArmyCharacter[] armyCharacters)
        {
            ClearCharacters();
            for (int i = 0; i < armyCharacters.Length; i++)
            {
                AddCharacter(armyCharacters[i]);
            }
        }

        public void ClearCharacters()
        {
            m_rockCharacters.Clear();
            m_paperCharacters.Clear();
            m_scissorCharacters.Clear();
        }

        public int GetTotalUnitPower(UnitType unitType) => GetTotalPower(GetCharactersOfUnityType(unitType));

        public int GetNumberOfCharacter(UnitType unitType) => GetCharactersOfUnityType(unitType).Count;

        private int GetTotalPower(List<ArmyCharacter> armyCharacters)
        {
            int power = 0;
            for (int i = 0; i < armyCharacters.Count; i++)
            {
                power += armyCharacters[i].power;
            }
            return power;
        }

        private List<ArmyCharacter> GetCharactersOfUnityType(UnitType unitType)
        {
            switch (unitType)
            {
                case UnitType.Rock:
                    return m_rockCharacters;
                case UnitType.Paper:
                    return m_paperCharacters;
                case UnitType.Scissors:
                    return m_scissorCharacters;
                default:
                    return null;
            }
        }

#if UNITY_EDITOR
        [SerializeField, AssetSelector, FoldoutGroup("Characters/Editor"), PropertyOrder(2)]
        private ArmyCharacter[] m_characterToAddList;


        [Button, ButtonGroup("Characters/Editor/Options"), PropertyOrder(3), ShowIf("@m_characterToAddList.Length > 0")]
        private void AddCharactersToComposition()
        {
            for (int i = 0; i < m_characterToAddList.Length; i++)
            {
                AddCharacter(m_characterToAddList[i]);
            }
            m_characterToAddList = new ArmyCharacter[0];
        }

        [Button, ButtonGroup("Characters/Editor/Options"), PropertyOrder(3), ShowIf("@m_characterToAddList.Length > 0")]
        private void SetCharactersToComposition()
        {
            SetCharacters(m_characterToAddList);
            m_characterToAddList = new ArmyCharacter[0];
        }
#endif
    }
}