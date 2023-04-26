using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players
{
    [System.Serializable]
    public class CombatArtLevelData : ICombatArtLevelDetail
    {
        [SerializeField, MinValue(0),TableColumnWidth(40,Resizable =false)]
        private int m_cost = 1;
        [SerializeField]
        private string m_description;
        [SerializeField, HideReferenceObjectPicker]
        private ICombatArtLevelConfiguration m_configuration;

        public CombatArtLevelData()
        {
            m_cost = 1;
            m_description = "";
            m_configuration = null;
        }

        public CombatArtLevelData(ICombatArtLevelConfiguration configuration)
        {
            m_cost = 1;
            m_description = "";
            m_configuration = configuration;
        }

        public int cost => m_cost;
        public string description => m_description;

        public ICombatArtLevelConfiguration configuration => m_configuration;

        public void SetConfiguration(ICombatArtLevelConfiguration configuration) => m_configuration = configuration;
    }
}