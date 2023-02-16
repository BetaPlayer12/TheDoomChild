using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [CreateAssetMenu(fileName = "ArmyAbilityList", menuName = "DChild/Gameplay/Army/Ability List")]
    public class ArmyAbilityList : SerializedScriptableObject
    {
        [SerializeField, AssetSelector(IsUniqueList = true)]
        private ArmyAbilityData[] m_armyAbilities;

        public ArmyAbilityData[] GetValidAbilities(params ArmyCategoryCompositionInfo[] categoryCompositionList)
        {
            List<ArmyAbilityData> validAbilities = new List<ArmyAbilityData>();
            for (int i = 0; i < categoryCompositionList.Length; i++)
            {
                var categoryComposition = categoryCompositionList[i];
                for (int k = 0; k < m_armyAbilities.Length; k++)
                {
                    var ability = m_armyAbilities[k];
                    var requirement = ability.requirement;
                    if (categoryComposition.characterCategory == requirement.characterCategory && categoryComposition.characterCount >= requirement.characterCount)
                    {
                        validAbilities.Add(ability);
                    }
                }
            }

            if (validAbilities.Count == 0)
                return null;

            return validAbilities.ToArray();
        }
    }
}