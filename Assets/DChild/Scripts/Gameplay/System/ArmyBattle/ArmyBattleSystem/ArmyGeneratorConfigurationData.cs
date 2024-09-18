﻿using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [CreateAssetMenu(fileName = "ArmyGeneratorConfigurationData", menuName = "DChild/Gameplay/Army/System/Army Generator Configuration Data")]
    public class ArmyGeneratorConfigurationData : SerializedScriptableObject
    {
        [SerializeField]
        private string m_armyName;
        [SerializeField]
        private ArmyGroupTemplateList m_generatableArmyGroup;
        [SerializeField]
        private Dictionary<ArmyGroupTemplateData, ArmyGroupTemplateData[]> m_replacementPair;

        public string armyName => m_armyName;

        public ArmyGroupTemplateList generatableArmyGroups => m_generatableArmyGroup;

        public ArmyGroupTemplateData[] GetGroupsToBeReplaced(ArmyGroupTemplateData groupToBeReplacedWith)
        {
            if (m_replacementPair.ContainsKey(groupToBeReplacedWith))
            {
                return m_replacementPair[groupToBeReplacedWith].ToArray();
            }
            return null;
        }
    }
}