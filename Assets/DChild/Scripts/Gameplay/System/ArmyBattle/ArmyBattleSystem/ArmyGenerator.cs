using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyGenerator : MonoBehaviour
    {
        [SerializeField]
        private ArmyGeneratorConfigurationData m_playerArmyConfiguration;

        private List<ArmyGroupTemplateData> m_cachedCreatedGroupsReference;

        public Army GenerateArmy(ArmyData armyData) => new Army(armyData.info);

        public Army GenerateArmy(RecruitedCharacterList characters)
        {
            var armyGroups = CreateViableArmyGroups(characters, m_playerArmyConfiguration.generatableArmyGroups);
            var filteredArmyGroups = RemoveReplacableArmyGroups(armyGroups, m_cachedCreatedGroupsReference);
            var troopCount = 0;
            for (int i = 0; i < filteredArmyGroups.Count; i++)
            {
                troopCount += filteredArmyGroups[i].GetTroopCount();
            }


            var armyInfo = new ArmyInfo(m_playerArmyConfiguration.armyName, troopCount, filteredArmyGroups.ToArray());
            return new Army(armyInfo);
        }

        private List<ArmyGroup> CreateViableArmyGroups(RecruitedCharacterList characters, ArmyGroupTemplateList referenceList)
        {
            m_cachedCreatedGroupsReference.Clear();
            List<ArmyGroup> createdArmyGroup = new List<ArmyGroup>();
            List<ArmyCharacterData> viableCharacters = new List<ArmyCharacterData>();
            for (int i = 0; i < referenceList.count; i++)
            {
                var armyGroup = referenceList.GetData(i);
                var characterGroup = armyGroup.armyCharacterGroup;
                viableCharacters = GetViableCharacters(characters, viableCharacters, characterGroup);

                if (viableCharacters.Count > 0)
                {
                    var newcharacterGroup = new ArmyCharacterGroup(characterGroup.name, viableCharacters.ToArray());
                    var newArmyGroup = new ArmyGroup(armyGroup.id, newcharacterGroup, armyGroup.damageType, armyGroup.specialSkill);
                    m_cachedCreatedGroupsReference.Add(armyGroup);
                    createdArmyGroup.Add(newArmyGroup);
                }
            }

            return createdArmyGroup;

            List<ArmyCharacterData> GetViableCharacters(RecruitedCharacterList characters, List<ArmyCharacterData> viableCharacters, ArmyCharacterGroup basedOnCharacterGroup)
            {
                viableCharacters.Clear();
                for (int j = 0; j < basedOnCharacterGroup.memberCount; j++)
                {
                    var character = basedOnCharacterGroup.GetCharacter(j);
                    if (characters.HasCharacter(basedOnCharacterGroup.GetCharacter(j)))
                    {
                        viableCharacters.Add(character);
                    }
                }
                return viableCharacters;
            }
        }

        private List<ArmyGroup> RemoveReplacableArmyGroups(List<ArmyGroup> toBeModified, List<ArmyGroupTemplateData> reference)
        {
            for (int i = 0; i < reference.Count; i++)
            {
                var replaceableArmyGroups = m_playerArmyConfiguration.GetGroupsToBeReplaced(reference[i]);
                if (replaceableArmyGroups != null)
                {
                    for (int j = 0; j < replaceableArmyGroups.Length; j++)
                    {
                        var referenceGroup = replaceableArmyGroups[j];
                        for (int k = toBeModified.Count - 1; k >= 0; k--)
                        {
                            if (referenceGroup.id == toBeModified[i].id)
                            {
                                toBeModified.RemoveAt(k);
                            }
                        }
                    }
                }
            }

            return toBeModified;
        }

        private void Awake()
        {
            m_cachedCreatedGroupsReference = new List<ArmyGroupTemplateData>();
        }
    }
}