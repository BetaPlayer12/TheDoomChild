using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using Sirenix.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay.Characters.Players
{

    [CreateAssetMenu(fileName = "CombatArtData", menuName = "DChild/Database/Combat Art Data")]
    public class CombatArtData : SerializedScriptableObject
    {
        [SerializeField, OnValueChanged("RenameFilename")]
        private CombatArt m_ability;
        [SerializeField]
        private string m_name;
#if UNITY_EDITOR
        [SerializeField, ValueDropdown("GetCombatArtConfigrationClasses"), OnValueChanged("OverrideConfigurations")]
        private string m_configurationType;
#endif
        [OdinSerialize, TableList(ShowIndexLabels = true), ListDrawerSettings(ShowIndexLabels = true)]
        private CombatArtLevelData[] m_levelDatas = new CombatArtLevelData[1];

        public CombatArt connectedCombatArt => m_ability;
        public string combatArtName => m_name;
        public int maxLevel => m_levelDatas.Length;

        private void RenameFilename()

        {

#if UNITY_EDITOR
            var assetPath = AssetDatabase.GetAssetPath(this);
            var name = m_ability.ToString().Replace(" ", "");
            AssetDatabase.RenameAsset(assetPath, $"{name}CombatArtData");
#endif
        }

#if UNITY_EDITOR
        private IEnumerable GetCombatArtConfigrationClasses()
        {
            return DChildUtility.GetDerivedInterfacesNames<ICombatArtLevelConfiguration>();
        }

        private void OverrideConfigurations()
        {
            var currentType = Type.GetType(m_configurationType);
            if (m_configurationType != null || m_levelDatas[0].configuration.GetType() != currentType)
            {
                for (int i = 0; i < m_levelDatas.Length; i++)
                {
                    m_levelDatas[i].SetConfiguration((ICombatArtLevelConfiguration)Activator.CreateInstance(currentType));
                }
            }
        }
#endif
    }
}